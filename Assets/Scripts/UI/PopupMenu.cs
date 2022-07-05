using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupMenu : ManagedMenu
{
    [Header("Popup Menu")]
    [SerializeField] public UnityEvent OnCloseEvents;

    public void ForceOpenPopup()
    {
        if (menuManager != null)
        {
            menuManager.RequestOpen(this);
        }
        else { Debug.LogWarning("Couldn't force open popup - no menu manager reference."); }
    }

    public override void ControlledClose()
    {
        if (OnCloseEvents != null)
        {
            OnCloseEvents.Invoke();
        }
        base.ControlledClose();
    }


}
