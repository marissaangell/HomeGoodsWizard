using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureEvolver : Interactable
{

    public override bool Interact(ToolType tool)
    {
        GlobalEventHandler.InvokeEvolveFurniture();
        return true;
    }

}
