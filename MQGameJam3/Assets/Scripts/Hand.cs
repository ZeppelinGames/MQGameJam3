using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform handTarget;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private float lerpSpeed = 5f;

    private Vector3 handTargetPos = Vector3.zero;

    [Header("Grabbing")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float handHeight = 0.25f;
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private AnimatorPoseSetter poseSetter;

    private Pickup holding;
    private bool grabbing = false;

    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit))
        {
            Vector3 handPos = hit.point + (Vector3.up * handHeight);
            handTarget.position = hit.point + new Vector3(0, 0.1f, 0);

            if (!grabbing)
            {
                handTargetPos = handPos;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // raycast down
            int setPose = 1;
            if (Physics.Raycast(hand.position, Vector3.down, out RaycastHit hitPickup, 10, grabLayer))
            {
                if (hitPickup.transform.TryGetComponent(out Pickup pick))
                {
                    pick.Grab();
                    holding = pick;
                    setPose = pick.SetPose;
                }

                if (hitPickup.transform.TryGetComponent(out GameObjectButton button))
                {
                    button.Click();
                }
            }

            handTargetPos = hit.point + new Vector3(0, heightOffset, 0);
            grabbing = true;
            poseSetter.SetPose(setPose);
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

        hand.position = Vector3.Lerp(hand.position, handTargetPos, Time.deltaTime * lerpSpeed);
        if (Vector3.Distance(hand.position, handTargetPos) < 0.05f)
        {
            grabbing = false;
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
