using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class Knife : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Vector3 pickUpRotation;
    [SerializeField] private Vector3 droppedRotation;
    [SerializeField] private float rotateSpeed = 3f;

    [Header("Cutting")]
    [SerializeField] private ChoppingBoard board;
    [SerializeField] private Vector3 cutPosition;
    [SerializeField] private float cutSpeed = 4f;
    [SerializeField] private float knifeSpeed = 10f;
    [SerializeField] private Transform knife;

    private Vector3 targetPos;

    private Vector3 targetRotation;
    private float lastCut;
    private Pickup pickup;

    private bool cutting = false;
    private bool stillCutting = false;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cutting)
        {
            targetPos = transform.position;
        }

        if (pickup.IsPickedUp)
        {
            if (Time.time - lastCut > (1 / cutSpeed))
            {
                if (Physics.Raycast(transform.position + cutPosition, Vector3.down, out RaycastHit hit))
                {
                    bool onBoard = false;
                    for (int i = 0; i < board.ChoppingItems.Length; i++)
                    {
                        if (hit.transform == board.ChoppingItems[i].transform)
                        {
                            onBoard = true;
                        }
                    }

                    if (onBoard)
                    {
                        if (hit.transform.TryGetComponent(out Cuttable c))
                        {
                            c.Cut();
                            lastCut = Time.time;

                            if (!cutting)
                            {
                                targetPos = hit.point - cutPosition;
                                cutting = true;
                                stillCutting = true;
                            }
                        }
                    }
                    else
                    {
                        stillCutting = false;
                    }
                }
            }


            if (Vector3.Distance(knife.position, targetPos) < 0.1f)
            {
                //targetPos = transform.position;
                cutting = false;
            }
        }
        else
        {
            stillCutting = false;
        }

        if (stillCutting)
        {
            knife.position = Vector3.Lerp(knife.position, targetPos, Time.deltaTime * knifeSpeed);
        }

        targetRotation = pickup.IsPickedUp ? pickUpRotation : droppedRotation;
        knife.rotation = Quaternion.Lerp(knife.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + cutPosition, 0.1f);
    }
}
