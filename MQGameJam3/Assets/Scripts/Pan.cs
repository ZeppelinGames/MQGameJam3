using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private float startCookTime;
    private Cookable cooking;

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
            startCookTime = Time.time;
            return;
        }

        if (cooking != null)
        {
            float cookingLevel = Mathf.Clamp01((Time.time - startCookTime) / cooking.CookTime);
            panSlider.value = cookingLevel;
            fillImage.color = cookingGradient.Evaluate(cookingLevel);
            cooking.SetCookLevel(cookingLevel);

            if (cookingLevel >= 1)
            {
                panSlider.value = 0;
                StoppedCooking();
            }
        }

        cols = Physics.OverlapBox(transform.position + insidePanOffset, new Vector3(insidePanRadius, panHeight, insidePanRadius));

        bool stillInPan = false;
        for (int i = 0; i < cols.Length; i++)
        {
            Collider collision = cols[i];
            if (!collision.gameObject.activeInHierarchy)
            {
                continue;
            }

            collision.transform.TryGetComponent(out Cookable c);
            if (c == cooking)
            {
                stillInPan = true;
                continue;
            }

            if (cooking == null && c != null)
            {
                startCookTime = Time.time;
                cooking = c;
                cooking.transform.SetParent(transform);
            }
            else
            {
                if (collision.transform.TryGetComponent(out Rigidbody rig))
                {
                    Debug.Log(rig.transform.name);
                    Vector2 rnd = Random.insideUnitCircle;
                    Vector3 launchDir = new Vector3(rnd.x, 5, rnd.y) * launchForce;
                    rig.AddForce(launchDir);
                }
            }
        }

        if (!stillInPan || cols.Length == 0)
        {
            StoppedCooking();
        }
    }

    void StoppedCooking()
    {
        if (cooking != null)
        {
            cooking.transform.SetParent(null);
            cooking = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + insidePanOffset, new Vector3(insidePanRadius, panHeight, insidePanRadius));
    }
}
