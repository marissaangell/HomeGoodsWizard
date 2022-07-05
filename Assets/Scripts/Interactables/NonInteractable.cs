using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonInteractable : Interactable
{
    public override bool Interact(ToolType tool)
    {
        return false;
    }
}
