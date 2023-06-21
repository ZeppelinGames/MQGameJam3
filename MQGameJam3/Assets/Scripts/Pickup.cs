using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{
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
}
