using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoUp : MonoBehaviour
{
    [SerializeField] private float torqueForce = 1f;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 rotationAxis = Vector3.Cross(transform.up, Vector3.up);
        rig.AddTorque(rotationAxis * torqueForce);
    }
}
