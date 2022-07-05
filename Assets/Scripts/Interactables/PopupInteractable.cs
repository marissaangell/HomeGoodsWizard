using UnityEngine;
using System.Collections.Generic;

public class PopupInteractable : Interactable
{
    [SerializeField] private List<PopupMenu> popupMenus;

    private PlayerInteractController playerRef;

    private int activePopupIdx = 0;

    public override void Target(PlayerInteractController player)
    {
        playerRef = player;
        base.Target(player);
    }

    public override bool Interact(ToolType tool)
    {
        PopupMenu popup = GetActivePopup();
        if (playerRef != null && popup != null)
        {
            popup.ForceOpenPopup();
            return true;
        }
        else
        {
            Debug.LogWarning("Couldn't interact with popup interactable!");
        }
        return false;
    }

    private PopupMenu GetActivePopup()
    {
        if (popupMenus.Count > activePopupIdx)
            return popupMenus[activePopupIdx];
        return null;
    }

    public void AdvanceActivePopupIdx()
    {
        activePopupIdx += 1;
    }
    

}
