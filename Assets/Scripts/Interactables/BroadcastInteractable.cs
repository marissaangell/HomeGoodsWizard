using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastInteractable : Interactable
{
    [Header("Broadcast Interactable")]
    [SerializeField] private bool _isInteractable = true;

    public delegate void InteractReceivedDelegate();
    public event InteractReceivedDelegate InteractReceivedEvent;

    public override bool Interact(ToolType tool)
    {
        if (IsInteractable(tool))
        {
            if (InteractReceivedEvent != null)
                InteractReceivedEvent.Invoke();
            return true;
        }
        return false;
    }

    public override bool IsInteractable(ToolType tool)
    {
        return _isInteractable;
    }

    public void SetInteractive(bool newState)
    {
        _isInteractable = newState;
    }


}
