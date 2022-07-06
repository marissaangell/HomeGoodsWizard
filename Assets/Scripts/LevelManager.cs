using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Workbench> workbenches = new List<Workbench>();

    [SerializeField] private List<FurnitureMapping> mappings = new List<FurnitureMapping>();

    [SerializeField] private bool debugUnhideFurniture = false;

    [SerializeField] private FurnitureDatabase furnitureDatabase;

    private void OnEnable()
    {
        foreach (Workbench workbench in workbenches)
        {
            workbench.furnitureSubmittedEvent += HandleFurnitureSubmission;
        }
    }

    private void OnDisable()
    {
        foreach (Workbench workbench in workbenches)
        {
            workbench.furnitureSubmittedEvent -= HandleFurnitureSubmission;
        }
    }

    void Start()
    {
        if (Debug.isDebugBuild && debugUnhideFurniture)
        {
            return;
        }

        // Hide all level mappings
        foreach (FurnitureMapping mapping in mappings)
        {
            mapping.levelInstance.SetActive(false);
        }

        // Unhide submitted furniture according to saved state
        foreach (string furnitureItemID in RecordKeeper.submittedFurniture)
        {
            UnhideFurnitureItem(furnitureItemID);
        }
    }

    private void HandleFurnitureSubmission(FurnitureItem item)
    {
        // Keep a record of handled items (prevent repeat work)
        RecordKeeper.LogSubmitted(item);

        Debug.Log("Submitting [" + item.displayName + "]...");
        UnhideFurnitureItem(item.ID);
    }

    private void UnhideFurnitureItem(string furnitureItemID)
    {
        foreach (FurnitureMapping mapping in mappings)
        {
            if (mapping.furnitureItem.ID.Equals(furnitureItemID))
            {
                mapping.levelInstance.SetActive(true);
                return;
            }
        }
        Debug.Log("No level mapping found for item ID [" + furnitureItemID + "]");
    }

}

[System.Serializable]
public struct FurnitureMapping
{
    public FurnitureItem furnitureItem;
    public GameObject levelInstance;
}