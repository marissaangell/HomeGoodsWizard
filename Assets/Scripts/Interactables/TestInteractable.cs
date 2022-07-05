using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public override bool Interact(ToolType tool)
    {
        Debug.Log("Interacted with " + gameObject.name + " using " + tool.ToString());
        return true;
    }
}
