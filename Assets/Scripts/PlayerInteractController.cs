using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private InteractUI _interactUI;

    [SerializeField] private List<Tool> tools;
    private int _activeToolIdx = 0;
    private Tool _activeTool;
    public ToolType ActiveToolType => _activeTool.type;

    private float raycastDistance = 5f;
    private Interactable _interactTarget;



    // Start is called before the first frame update
    void Start()
    {
        if (tools.Count == 0)
        {
            Debug.LogWarning("Player has no usable tools!");
        }
        else
        {
            _activeTool = tools[_activeToolIdx];
            _interactUI.UpdateActiveTool(_activeTool);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, raycastDistance, _interactMask))
        {
            Interactable hitInteractable = hitInfo.collider.GetComponentInChildren<Interactable>();
            if (hitInteractable != null && hitInteractable != _interactTarget)
            {
                UpdateInteractTarget(hitInteractable);
            }

        }
        else if (_interactTarget != null) //raycast no longer hitting an interact target => untarget current target
        {
            VoidCurrentInteractTarget();
        }
    }

    void OnInteract(InputValue value)
    {
        if (_interactTarget != null)
        {
            _interactTarget.Interact(_activeTool.type);
        }
    }

    private void UpdateInteractTarget(Interactable interactable)
    {
        //Untarget old interact target if it exists
        if (_interactTarget != null) { _interactTarget.Untarget(); }

        //Target new interact target
        _interactTarget = interactable;
        _interactTarget.Target(this);

        //Update Interact UI
        if (interactable.IsInteractable(this.ActiveToolType))
        {
            _interactUI.UpdateActiveTool(_activeTool);
            _interactUI.UpdatePromptText(interactable.GetDisplayText(), interactable.ShouldDisplayToolText());

            _interactUI.ShowInteractPrompt();
        }
        else
        {
            _interactUI.ShowCantInteractUI(interactable.GetDisplayText(), interactable.ShouldDisplayToolText());
        }
    }

    private void VoidCurrentInteractTarget()
    {
        _interactTarget.Untarget();
        _interactTarget = null;

        _interactUI.UpdatePromptText(string.Empty);
        _interactUI.HideInteractPrompt();
    }

    void OnNextTool(InputValue value)
    {
        NextTool();
    }

    private void NextTool()
    {
        int nextToolIdx = _activeToolIdx + 1;
        if (nextToolIdx >= tools.Count) { nextToolIdx = 0; }

        _activeToolIdx = nextToolIdx;
        _activeTool = tools[nextToolIdx];

        _interactUI.UpdateActiveTool(_activeTool);

        // Update interact prompt if it's visible
        if (_interactTarget != null)
        {
            _interactTarget.Untarget();
            _interactTarget.Target(this);
            _interactUI.UpdatePromptText(_interactTarget.GetDisplayText(), _interactTarget.ShouldDisplayToolText());
        }
    }

    void OnPreviousTool(InputValue value)
    {
        PrevTool();
    }

    private void PrevTool()
    {
        int nextToolIdx = _activeToolIdx - 1;
        if (nextToolIdx < 0) { nextToolIdx = tools.Count - 1; }

        _activeToolIdx = nextToolIdx;
        _activeTool = tools[nextToolIdx];

        _interactUI.UpdateActiveTool(_activeTool);

        // Update interact prompt if it's visible
        if (_interactTarget != null)
        {
            _interactUI.UpdatePromptText(_interactTarget.GetDisplayText(), _interactTarget.ShouldDisplayToolText());
            _interactTarget.Target(this);
        }
    }


}
