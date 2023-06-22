using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Pan : MonoBehaviour
{
    [SerializeField] private float launchForce = 500;
    [SerializeField] private Vector3 insidePanOffset;
    [SerializeField] private float insidePanRadius;
    [SerializeField] private float panHeight = 0.1f;

    [Header("UI")]
    [SerializeField] private Slider panSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient cookingGradient;

    private Pickup[] prevItems;
    //private float startCookTime;
    //private Cookable cooking;

    private List<Cookable> cooking = new List<Cookable>();
    private List<float> startCookTime = new List<float>();
    private bool[] stillInPan;

    private Collider[] cols = new Collider[0];

    private bool panOn = false;

    private void Start()
    {
        panSlider.value = 0;
    }

    public void SetPanState(bool isOn)
    {
        panOn = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (!panOn)
        {
            for (int i = 0; i < cooking.Count; i++)
            {
                startCookTime[i] = Time.time;
            }
        }

        if (cooking.Count > 0)
        {
            float earliestStart = 10000;
            float latestEnd = 0;

            for (int i = 0; i < cooking.Count; i++)
            {
                float cookingLevel = Mathf.Clamp01((Time.time - startCookTime[i]) / cooking[i].CookTime);
                cooking[i].SetCookLevel(cookingLevel);

                if (startCookTime[i] < earliestStart)
                {
                    earliestStart = startCookTime[i];
                }

                float endTime = startCookTime[i] + cooking[i].CookTime;
                if (endTime > latestEnd)
                {
                    latestEnd = endTime;
                }

                if (cookingLevel >= 1)
                {
                    StoppedCooking(cooking[i]);
                }
            }

            float allCookLevel = Mathf.Clamp01((Time.time - earliestStart) / (latestEnd - earliestStart));
            panSlider.value = allCookLevel;
            fillImage.color = cookingGradient.Evaluate(allCookLevel);
        }

        cols = Physics.OverlapBox(transform.position + insidePanOffset, new Vector3(insidePanRadius, panHeight, insidePanRadius));
        List<Pickup> prevPicks = new List<Pickup>();

        stillInPan = new bool[cooking.Count + cols.Length];
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent(out Pickup p))
            {
                prevPicks.Add(p);
                p.transform.SetParent(transform);

                if (cols[i].TryGetComponent(out Cookable c))
                {
                    if (!cooking.Contains(c))
                    {
                        startCookTime.Add(Time.time);
                        cooking.Add(c);
                        stillInPan[cooking.IndexOf(c)] = true;
                    }

                    if (cooking.Contains(c))
                    {
                        stillInPan[cooking.IndexOf(c)] = true;
                    }
                }
            }
        }

        for (int i = 0; i < cooking.Count; i++)
        {
            if (!stillInPan[i])
            {
                StoppedCooking(cooking[i]);
            }
        }

        if (cooking.Count == 0)
        {
            panSlider.value = 0;
        }

        if (prevItems != null)
        {
            for (int i = 0; i < prevItems.Length; i++)
            {
                bool hasItem = false;
                for (int j = 0; j < cols.Length; j++)
                {
                    if (cols[j].transform == prevItems[i].transform)
                    {
                        hasItem = true;
                    }
                }

                if (!hasItem)
                {
                    prevItems[i].transform.SetParent(null);
                }
            }
        }

        prevItems = prevPicks.ToArray();
    }

    void StoppedCooking(Cookable c)
    {
        c.transform.SetParent(null);
        startCookTime.RemoveAt(cooking.IndexOf(c));
        cooking.Remove(c);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + insidePanOffset, new Vector3(insidePanRadius, panHeight, insidePanRadius));
    }
}
