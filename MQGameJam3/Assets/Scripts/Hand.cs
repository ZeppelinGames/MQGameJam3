using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform holdPoint;

    [Header("Grabbing")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float handHeight = 0.25f;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private AnimatorPoseSetter poseSetter;

    private Pickup holding;

    private Vector3 prevPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (hand.position - prevPos).normalized;

        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit))
        {
            Vector3 handPos = hit.point + (Vector3.up * handHeight);
            hand.position = handPos;
        }

        prevPos = hand.position;


        if (Input.GetMouseButtonDown(0))
        {
            // raycast down
            if (Physics.Raycast(hand.position, Vector3.down, out RaycastHit hitPickup))
            {
                if (hitPickup.transform.gameObject.IsInLayerMask(grabLayer))
                {
                    if (hitPickup.transform.TryGetComponent(out Pickup pick))
                    {
                        pick.Grab();
                        holding = pick;
                    }
                }

                if (hitPickup.transform.TryGetComponent(out GameObjectButton button))
                {
                    button.Click();
                }
            }

            poseSetter.SetPose(1);
        }

        if (holding != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                // drop item
                holding.Drop();
                holding = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            poseSetter.SetPose(0);
        }
    }

    private void FixedUpdate()
    {
        if (holding != null)
        {
            Vector3 moveDirection = (holdPoint.position - (holding.transform.position + holding.pickupOffset)).normalized;
            Vector3 targetPosition = holdPoint.position - holding.pickupOffset;
            Vector3 force = moveDirection * moveSpeed;

            if (Vector3.Distance(holding.transform.position, targetPosition) > 0.1f)
            {
                holding.Rig.AddForce(force);
                holding.transform.position = Vector3.Lerp(holding.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            }
            else
            {
                holding.transform.position = targetPosition;
            }
        }
    }
}
