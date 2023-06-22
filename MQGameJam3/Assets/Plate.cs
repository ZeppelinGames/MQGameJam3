using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 size;

    private Collider[] cols;
    private Pickup[] prevCols;

    public Pickup[] PlatedItems { get => prevCols; }

    // Update is called once per frame
    void Update()
    {
        cols = Physics.OverlapBox(transform.position + offset, size);
        List<Pickup> prevPicks = new List<Pickup>();
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].transform != this.transform && !cols[i].gameObject.isStatic && cols[i].TryGetComponent(out Pickup p) && !p.CompareTag("Pan"))
            {
                cols[i].transform.SetParent(transform);
                prevPicks.Add(p);
            }
        }

        if (prevCols != null)
        {
            for (int i = 0; i < prevCols.Length; i++)
            {
                bool stillIn = false;
                for (int j = 0; j < cols.Length; j++)
                {
                    if (prevCols[i].transform == cols[j].transform)
                    {
                        stillIn = true;
                    }
                }

                if (!stillIn)
                {
                    prevCols[i].transform.SetParent(null);
                }
            }
        }
        prevCols = prevPicks.ToArray();
    }   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
