using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cuttable : MonoBehaviour
{
    [SerializeField] private int maxCuts = 4;
    [SerializeField] private GameObject cutObject;
    private MeshRenderer mr;

    float cutAmount = 1;
    int currCuts = 0;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Cut()
    {
        if (currCuts < maxCuts)
        {
            GameObject newCut = Instantiate(cutObject);
            newCut.transform.position = transform.position + (transform.forward * 0.5f);
            newCut.transform.eulerAngles = new Vector3(-60, newCut.transform.eulerAngles.y, newCut.transform.eulerAngles.z);

            cutAmount -= (1 / maxCuts);
            mr.material.SetFloat("_CutAmount", cutAmount);

            currCuts++;
            if (currCuts == maxCuts)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
