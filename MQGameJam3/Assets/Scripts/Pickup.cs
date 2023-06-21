using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private Vector3 pickupPoint;
    public Vector3 pickupOffset { get => pickupPoint; }

    public Rigidbody Rig { get => rig; }
    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
        rig.useGravity = false;
    }

    public void Drop()
    {
        rig.useGravity = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + pickupPoint, 0.1f);
    }
}
