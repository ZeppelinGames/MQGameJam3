using System;
using UnityEngine;

public static class Helpers
{
    public static bool IsInLayerMask(this GameObject go, LayerMask mask)
    {
        return mask == (mask | (1 << go.layer));
    }

    public static bool TryGetComponentInParent<T>(this GameObject g, out T t) where T : MonoBehaviour
    {
        T newT = g.GetComponentInParent<T>();
        t = newT;
        return t != null;
    }
}

