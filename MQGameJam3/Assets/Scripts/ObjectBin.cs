using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBin : MonoBehaviour
{
    [SerializeField] private string[] ignoreTags;

    private void OnCollisionEnter(Collision collision)
    {
        if (!HasTag(collision.gameObject) && collision.transform.TryGetComponent(out Pickup _))
        {
            Destroy(collision.gameObject);
        }
    }

    bool HasTag(GameObject g)
    {
        for (int i = 0; i < ignoreTags.Length; i++)
        {
            if (g.CompareTag(ignoreTags[i]))
            {
                return true;
            }
        }
        return false;
    }
}
