using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class FurnitureComponent : Interactable
{
    public FurnitureController furnitureController;
    [SerializeField] private string locationName;
    [SerializeField] private List<ToolType> incompatibleTools;
    

    private bool componentCurrentlyTargeted = false;
    private ToolType lastActiveToolType = ToolType.UNDEFINED;

    public string LocationName => locationName;

    public void Awake()
    {
        if (furnitureController == null)
        {
            try
            {
                furnitureController = gameObject.GetComponentInParent<FurnitureController>();
            }
            catch
            {
                Debug.LogWarning("[" + gameObject.name + "]: furniture controller reference not set, and could not be set automatically");
            }
        }

        if (_outline == null)
        {
            try
            {
                _outline = gameObject.GetComponent<Outline>();
                _outline.enabled = false;
            }
            catch
            {
                Debug.LogWarning("[" + gameObject.name + "]: outline reference not set, and could not be set automatically");
            }
        }
    }

    public override bool IsInteractable(ToolType tool)
    {
        return !incompatibleTools.Contains(tool);
    }

    public override bool Interact(ToolType tool)
    {
        if (IsInteractable(tool))
        {
            furnitureController.LogInteraction(tool, this);
            return true;
        }
        return false;
    }

    public override void Target(PlayerInteractController player)
    {
        if (furnitureController.ShouldTargetComponent(player.ActiveToolType))
        {
            componentCurrentlyTargeted = true;
            _outline.OutlineColor = incompatibleTools.Contains(player.ActiveToolType) ? Color.red : Color.white;
            base.Target(player);
        }
        else if (componentCurrentlyTargeted) //Transfering targeting from component to overall object
        {
            componentCurrentlyTargeted = false;
            _outline.enabled = false;

        }
        lastActiveToolType = player.ActiveToolType;
    }

    public override void Untarget()
    {
        furnitureController.LogUntarget();

        if (componentCurrentlyTargeted)
            base.Untarget();
    }

    public override string GetDisplayText()
    {
        if (furnitureController.WaitingToEvolve)
        {
            if (lastActiveToolType == ToolType.Erase)
                return "Erase interaction history from " + furnitureController.ObjectName;
            return "This item is ready to evolve!";
        }
        else if (componentCurrentlyTargeted)
        {
            return this.promptMessage;
        }
        else
        {
            return furnitureController.ObjectName;
        }
    }

    public override bool ShouldDisplayToolText()
    {
        if (furnitureController.WaitingToEvolve)
        {
            return false;
        }
        else
        {
            return useTool;
        }
    }

    

}

