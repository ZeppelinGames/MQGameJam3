using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Egg : MonoBehaviour
{
    [SerializeField] private GameObject wetEgg;
    [SerializeField] private GameObject eggHalf;
    [SerializeField] private float breakMagnitude = 5f;
    [SerializeField] private float explosionForce = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakMagnitude)
        {
            GameObject newWetEgg = Instantiate(wetEgg);
            newWetEgg.transform.position = transform.position;

            GameObject egghalf = Instantiate(eggHalf);
            egghalf.transform.position = transform.position - (transform.forward * 0.1f);
            if (eggHalf.TryGetComponent(out Rigidbody eggRig))
            {
               eggRig.AddForce(Vector3.up * explosionForce);
            }

            GameObject egghalf2 = Instantiate(eggHalf);
            egghalf2.transform.position = transform.position + (transform.forward * 0.1f);
            egghalf2.transform.eulerAngles = new Vector3(0, 0, 180);
            if (eggHalf.TryGetComponent(out Rigidbody eggRig2))
            {
                eggRig2.AddForce(Vector3.up * explosionForce);
            }

            this.gameObject.SetActive(false);
        }
    }
}
