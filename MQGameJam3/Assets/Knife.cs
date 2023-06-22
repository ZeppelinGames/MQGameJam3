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
    [SerializeField] private Vector3 cutPosition;
    [SerializeField] private float cutSpeed = 4f;
    [SerializeField] private Transform knife;

    private Vector3 targetPos;

    private Vector3 targetRotation;
    private float lastCut;
    private Pickup pickup;

    private bool cutting = false;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup.IsPickedUp)
        {
            if (Time.time - lastCut > (1 / cutSpeed))
            {
                if (Physics.Raycast(transform.position + cutPosition, Vector3.down, out RaycastHit hit))
                {
                    if (hit.transform.TryGetComponent(out Cuttable c))
                    {
                        c.Cut();
                        lastCut = Time.time;

                        if (!cutting)
                        {
                            targetPos = hit.point - new Vector3(0, 0.1f, 0);
                            cutting = true;
                        }
                    }
                }
            }

            knife.position = Vector3.Lerp(knife.position, targetPos, Time.deltaTime * cutSpeed);

            if (Vector3.Distance(knife.position, targetPos) < 0.1f)
            {
                targetPos = transform.position;
                cutting = false;
            }
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
