using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    [SerializeField] private Vector3 chopOffset;
    [SerializeField] private Vector3 chopSize;

    public Collider[] ChoppingItems { get => cols; }
    private Collider[] cols;

    // Update is called once per frame
    void Update()
    {
        cols = Physics.OverlapBox(transform.position + chopOffset, chopSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + chopOffset, chopSize);
    }
}
