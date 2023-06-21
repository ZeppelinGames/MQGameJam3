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
            holding.Rig.MovePosition(Vector3.Slerp(holding.transform.position, holdPoint.position - holding.pickupOffset, Time.deltaTime * moveSpeed));
            //holding.transform.position = hand.position;

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
}
