using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Workbench> workbenches = new List<Workbench>();

    [SerializeField] private List<FurnitureMapping> mappings = new List<FurnitureMapping>();

    [SerializeField] private bool debugUnhideFurniture = false;

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
    }

    private void HandleFurnitureSubmission(FurnitureItem item)
    {
        // Keep a record of handled items (prevent repeat work)
        if (RecordKeeper.submittedFurniture.Contains(item))
            return;
        RecordKeeper.submittedFurniture.Add(item);

        Debug.Log("Submitting [" + item.displayName + "]...");

        foreach (FurnitureMapping mapping in mappings)
        {
            if (mapping.furnitureItem.Equals(item))
            {
                mapping.levelInstance.SetActive(true);
                return;
            }
        }

        Debug.Log("No level mapping found for [" + item.displayName + "]");
    }

}

[System.Serializable]
public struct FurnitureMapping
{
    public FurnitureItem furnitureItem;
    public GameObject levelInstance;
}