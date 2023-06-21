using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectButton : MonoBehaviour
{
    [SerializeField] private UnityEvent buttonAction;

    public void Click()
    {
        buttonAction?.Invoke();
    }
}
