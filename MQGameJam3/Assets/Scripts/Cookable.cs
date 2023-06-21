using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cookable : MonoBehaviour
{
    [SerializeField] private GameObject cookedItem;
    [SerializeField] private float cookTime = 5f;
    public float CookTime { get => cookTime; }

    private Rigidbody rig;
    public Rigidbody Rig { get => rig; }

    private float cookForce = 100;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    public void SetCookLevel(float v)
    {
        if (v >= 1)
        {
            GameObject cooked = Instantiate(cookedItem);
            cooked.transform.position = transform.position;

            if(cooked.TryGetComponent(out Rigidbody rig))
            {
                rig.AddForce(Vector3.up * cookForce);
            }

            this.gameObject.SetActive(false);
        }
    }
}