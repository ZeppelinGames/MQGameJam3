using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlatingStation : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 size;
    [SerializeField] private TextMeshProUGUI nameText;

    private Collider[] cols;

    private Plate currPlate;
    private float plateTime;
    private bool hasGenedName = true;

    static string[] countNames =
    {
        "single",
        "double",
        "triple"
    };

    static string[] descs =
    {
        "deluxe",
        "supreme",
        "premium",
        "gourmet",
        "delight",
    };


    private Func<Pickup[], string>[] namingFuncs = {
        (Pickup[] p) => {
            string most = getMostCommonItem(p, out int c);
            if(c == 0)
            {
                return "Where's my food?";
            }
            string count = c <= 0 || c - 1 >= countNames.Length ? "multiple" : countNames[c - 1];
            
            return $"The {count} {most} {descs[UnityEngine.Random.Range(0, descs.Length)]}";
        },
        (Pickup[] p) => {
            string most = getMostCommonItem(p, out int c);
            if(c == 0)
            {
                return "Thats a plate.";
            }
            return $"The too many {most}s {descs[UnityEngine.Random.Range(0, descs.Length)]}";
        },
     };

    // Update is called once per frame
    void Update()
    {
        cols = Physics.OverlapBox(transform.position + offset, size);

        bool hasPlate = false;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out Plate p))
            {
                if (currPlate != p)
                {
                    hasGenedName = false;
                    currPlate = p;
                    plateTime = Time.time;
                    hasPlate = true;
                }
                else
                {
                    hasPlate = true;
                }
            }
        }
        if(!hasPlate)
        {
            currPlate = null;
            hasGenedName = false;
        }

        if (currPlate != null && !hasGenedName)
        {
            if (Time.time - plateTime > 1)
            {
                nameText.text = namingFuncs[UnityEngine.Random.Range(0, namingFuncs.Length)](currPlate.PlatedItems);
                hasGenedName = true;
            }
        }

        if (currPlate == null)
        {
            nameText.text = "";
        }
    }

    static string getMostCommonItem(Pickup[] items, out int count)
    {
        Debug.Log("Got " + items.Length);
        Dictionary<Pickup, int> itemCount = new Dictionary<Pickup, int>();
        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log(items[i].ItemName);
            if (itemCount.ContainsKey(items[i]))
            {
                itemCount[items[i]]++;
            }
            else
            {
                itemCount.Add(items[i], 1);
            }
        }

        int highest = 0;
        Pickup p = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (itemCount[items[i]] > highest)
            {
                Debug.Log(items[i] + " " + highest);
                p = items[i];
                highest = itemCount[items[i]];
            }
        }
        count = highest;

        return p == null ? "nothing " : p.ItemName;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
