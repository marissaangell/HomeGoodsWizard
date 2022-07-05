using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventHandler
{
    public delegate void EvolveFurniture();
    public static event EvolveFurniture EvolveFurnitureEvent;

    public static void InvokeEvolveFurniture()
    {
        EvolveFurnitureEvent.Invoke();
    }
}
