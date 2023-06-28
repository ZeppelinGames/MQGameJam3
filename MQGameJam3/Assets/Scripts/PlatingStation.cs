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
            return $"The {descs[Random.Range(0, descs.Length)]} {most}";
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

                if (currPlate.PlatedItems != null && currPlate.PlatedItems.Length >= 1)
                {
                    string n = getProcessedName();
                    if (n != null)
                    {
                        newName = n;
                    }
                }

                nameText.text = newName;
                hasGenedName = true;
            }
        }
    }

    string getProcessedName()
    {
        bool isSalad = true;
        bool isSandwich = false;
        bool isToast = false;

        Dictionary<string, int> itemCounts = getItemCounts(currPlate.PlatedItems);
        foreach (string key in itemCounts.Keys)
        {
            int value = itemCounts[key];

            if (key == "knife")
            {
                string[] res =
                {
                    "that's a knife",
                    "put that back",
                    "careful. that's sharp",
                    "ow!"
                };
                return res[Random.Range(0, res.Length)];
            }

            if (key == "pan")
            {
                string[] res =
                {
                    "that's a pan",
                    "put that back",
                    "careful. that's hot"
                };
                return res[Random.Range(0, res.Length)];
            }

            if (key == "plate")
            {
                string[] res =
                {
                    "why's there a plate in there",
                    "don't drop it"
                };
                return res[Random.Range(0, res.Length)];
            }

            if (key == "bread" && value >= 2)
            {
                isSandwich = true;
            }

            if ((key == "bread" || key == "toast") && value > 0)
            {
                isSalad = false;
            }

            if (key == "toast")
            {
                isToast = true;
                isSandwich = (value > 1) ? true : isSandwich;
            }
        }

        string dishType = "";
        string common = getMostCommonItem(currPlate.PlatedItems, out int _);

        if (isSalad)
        {
            dishType = "salad";
        }

        if (isToast)
        {
            dishType = "toast";
        }
        if (isSandwich)
        {
            dishType = "sandwich";
        }
        if (isToast && isSandwich)
        {
            dishType = "toasted sandwich";
        }

        int highest = 0;
        if (isToast || isSandwich)
        {
            // get most common, non bread item
            Pickup p = null;
            for (int i = 0; i < currPlate.PlatedItems.Length; i++)
            {
                string iName = currPlate.PlatedItems[i].ItemName;
                if ((iName != "bread" && iName != "toast") && itemCounts[iName] > highest)
                {
                    p = currPlate.PlatedItems[i];
                    highest = itemCounts[iName];
                }
            }

            common = p == null ? "plain" : p.ItemName;
        }

        string count = highest <= 0 || highest - 1 >= countNames.Length ? "multiple" : countNames[highest - 1];

        string[] options =
        {
            $"{descs[Random.Range(0, descs.Length)]} {common} {dishType}",
            $"{count} {common} {dishType}"
        };

        return options[Random.Range(0, options.Length)];
    }

    static Dictionary<string, int> getItemCounts(Pickup[] items)
    {
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

        return itemCount;
    }

    static string getMostCommonItem(Pickup[] items, out int count)
    {
        if(items == null || items.Length == 0)
        {
            count = 0;
            return "nothing";
        }

        Debug.Log("Got " + items.Length);
        Dictionary<string, int> itemCount = getItemCounts(items);

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
