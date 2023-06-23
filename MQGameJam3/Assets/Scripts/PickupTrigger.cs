using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupTrigger : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private UnityEvent triggerEvent;

    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (other.TryGetComponent(out Pickup p))
        {
            Debug.Log(p.ItemName);
            if (p.ItemName.Equals(itemID))
            {
                triggerEvent.Invoke();
                hasTriggered = true;
            }
        }
    }
}
