using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

using Random = UnityEngine.Random;

public class PlatingStation : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 size;
    [SerializeField] private TextMeshProUGUI nameText;

    private Collider[] cols;
    private Pickup[] prevPlateItems;

    private Plate currPlate;
    private bool hasGenedName;

    static string[] countNames =
    {
        "single",
        "double",
        "triple",
        "quadruple"
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
            
            return $"The {count} {most} {descs[Random.Range(0, descs.Length)]}";
        },
        (Pickup[] p) => {
            string most = getMostCommonItem(p, out int c);
            if(c == 0)
            {
                return "Thats a plate.";
            }
            return $"The too many {most}s {descs[Random.Range(0, descs.Length)]}";
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
                    currPlate = p;
                }

                hasPlate = true;
            }
        }

        if (!hasPlate)
        {
            currPlate = null;
            hasGenedName = false;
            nameText.text = "";
        }

        if (currPlate != null)
        {
            if (prevPlateItems != null && currPlate.PlatedItems != null)
            {
                if (prevPlateItems.Length != currPlate.PlatedItems.Length)
                {
                    hasGenedName = false;
                }
                else
                {
                    List<Pickup> currItems = new List<Pickup>();
                    currItems.AddRange(currPlate.PlatedItems);

                    int itemCount = 0;

                    for (int i = 0; i < prevPlateItems.Length; i++)
                    {
                        int iIndex = -1;
                        for (int j = 0; j < currItems.Count; j++)
                        {
                            if (prevPlateItems[i] == currItems[j])
                            {
                                iIndex = j;
                            }
                        }

                        if (iIndex >= 0)
                        {
                            itemCount++;
                            currItems.RemoveAt(iIndex);
                        }
                    }

                    if (itemCount != prevPlateItems.Length)
                    {
                        Debug.Log("Updated plate");
                        hasGenedName = false;
                    }
                }
            }
            prevPlateItems = currPlate.PlatedItems;

            if (!hasGenedName)
            {
                string newName = namingFuncs[Random.Range(0, namingFuncs.Length)](currPlate.PlatedItems);
                if (currPlate.PlatedItems != null && currPlate.PlatedItems.Length == 1)
                {
                    switch (currPlate.PlatedItems[0].ItemName)
                    {
                        case "knife":
                            newName = "Uhhhh. That's a knife";
                            break;
                        case "pan":
                            newName = "You need to cook with that";
                            break;
                        case "egg":
                            string[] eggNames =
                            {
                                "egg",
                                "wow. delicate hands"
                            };

                            newName = eggNames[Random.Range(0, eggNames.Length)];
                            break;
                    }
                }

                nameText.text = newName;
                hasGenedName = true;
            }
        }
    }

    static string getMostCommonItem(Pickup[] items, out int count)
    {
        if(items == null || items.Length == 0)
        {
            count = 0;
            return "nothing";
        }

        Debug.Log("Got " + items.Length);
        Dictionary<string, int> itemCount = new Dictionary<string, int>();
        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log(items[i].ItemName);
            if (itemCount.ContainsKey(items[i].ItemName))
            {
                itemCount[items[i].ItemName]++;
            }
            else
            {
                itemCount.Add(items[i].ItemName, 1);
            }
        }

        int highest = 0;
        Pickup p = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (itemCount[items[i].ItemName] > highest)
            {
                p = items[i];
                highest = itemCount[items[i].ItemName];
            }
        }
        count = itemCount[p.ItemName];

        return p == null ? "nothing " : p.ItemName;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
