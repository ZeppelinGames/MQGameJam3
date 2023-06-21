using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stovetop : MonoBehaviour
{
    [SerializeField] private Vector3 heatOffset = Vector3.zero;
    [SerializeField] private Vector3 heatSize = Vector3.one;
    [SerializeField] private string panTag = "Pan";

    /*    private List<Pan> pans = new List<Pan>();
        private List<bool> pansOn = new List<bool>();
    */
    private List<Pan> allPans = new List<Pan>();
    private Dictionary<Pan, bool> pans = new Dictionary<Pan, bool>();

    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapBox(transform.position + heatOffset, heatSize);

        for (int i = 0; i < allPans.Count; i++)
        {
            pans[allPans[i]] = false;
        }

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag(panTag))
            {
                if (cols[i].gameObject.TryGetComponentInParent(out Pan pan))
                {
                    if (!pans.ContainsKey(pan))
                    {
                        allPans.Add(pan);
                        pans.Add(pan, true);
                        pan.SetPanState(true);
                    }
                    else
                    {
                        pans[pan] = true;
                    }
                }
            }
        }

        for (int i = 0; i < allPans.Count; i++)
        {
            allPans[i].SetPanState(pans[allPans[i]]);

            if (!pans[allPans[i]])
            {
                pans.Remove(allPans[i]);
                allPans.RemoveAt(i);
                i--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + heatOffset, heatSize);
    }
}
