using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private Vector3 pickupPoint;
    public Vector3 pickupOffset { get => pickupPoint; }


    public bool IsPickedUp { get => isPickedUp; }
    private bool isPickedUp;

    public Rigidbody Rig { get => rig; }
    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
        rig.useGravity = false;
        isPickedUp = true;
    }

    public void Drop()
    {
        rig.velocity = Vector3.zero;
        rig.useGravity = true;

        isPickedUp = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + pickupPoint, 0.1f);
    }
}
