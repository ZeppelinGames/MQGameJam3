using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform hand;
    [SerializeField] private float handHeight = 0.25f;
    [SerializeField] private float lookAheadDist = 0.1f;
    [SerializeField] private LayerMask grabLayer;

    private Transform holding;

    private Vector3 prevPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (hand.position - prevPos).normalized;

        Vector3 lookAhead = dir * lookAheadDist;
        Vector3 screenPos = cam.WorldToScreenPoint(lookAhead);

        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition + screenPos);

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
                    holding = hitPickup.transform;
                }
            }
        }

        if(holding != null)
        {
            holding.transform.position = hand.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // drop item
            holding = null;
        }
    }
}
