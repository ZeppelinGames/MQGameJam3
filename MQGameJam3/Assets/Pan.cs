using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pan : MonoBehaviour
{
    [SerializeField] private float launchForce = 500;
    [SerializeField] private Vector3 insidePanOffset;
    [SerializeField] private float insidePanRadius;
    
    [Header("UI")]
    [SerializeField] private Slider panSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient cookingGradient;

    private float startCookTime;
    private Cookable cooking;


    private void Start()
    {
        panSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooking != null)
        {
            float cookingLevel = Mathf.Clamp01((Time.time - startCookTime) / cooking.CookTime);
            panSlider.value = cookingLevel;
            fillImage.color = cookingGradient.Evaluate(cookingLevel);
            cooking.SetCookLevel(cookingLevel);
        }

        Collider[] cols = Physics.OverlapSphere(transform.position + insidePanOffset, insidePanRadius);

        if (cols.Length == 0)
        {
            cooking = null;
        }

        foreach (Collider collision in cols)
        {
            if (cooking == null && collision.transform.TryGetComponent(out Cookable c))
            {
                startCookTime = Time.time;
                cooking = c;
            }
            else
            {
                if (collision.transform.TryGetComponent(out Cookable c2))
                {
                    if (c2 == cooking)
                    {
                        continue;
                    }
                }

                if (collision.transform.TryGetComponent(out Rigidbody rig))
                {
                    Debug.Log(rig.transform.name);
                    Vector2 rnd = Random.insideUnitCircle;
                    Vector3 launchDir = new Vector3(rnd.x, 5, rnd.y) * launchForce;
                    rig.AddForce(launchDir);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + insidePanOffset, insidePanRadius);
    }
}
