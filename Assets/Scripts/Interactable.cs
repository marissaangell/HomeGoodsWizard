using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    protected Outline _outline;
    [SerializeField] protected string promptMessage;
    [SerializeField] protected bool useTool;

    public virtual string GetDisplayText()
    {
        return promptMessage;
    }

    public virtual bool ShouldDisplayToolText()
    {
        return useTool;
    }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public virtual bool IsInteractable(ToolType tool)
    {
        return true;
    }

    public virtual void Target(PlayerInteractController player)
    {
        _outline.enabled = true;
    }

    public virtual void Untarget()
    {
        _outline.enabled = false;
    }

    public abstract bool Interact(ToolType tool);

}
