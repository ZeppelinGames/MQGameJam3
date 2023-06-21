using System;
using UnityEngine;

public static class Helpers
{
    public static bool IsInLayerMask(this GameObject go, LayerMask mask)
    {
        return mask == (mask | (1 << go.layer));
    }
}

