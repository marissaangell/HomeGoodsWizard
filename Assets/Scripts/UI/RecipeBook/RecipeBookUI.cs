using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecipeBookUI : ManagedMenu
{
    [Header("Recipe Database")]
    [SerializeField] public FurnitureDatabase furnitureDatabase;

    [Header("Recipe Book UI Root")]
    [SerializeField] private GameObject recipeBookUIRoot;

    [Header("Furniture List UI References")]
    [SerializeField] private GameObject furnitureEntryTemplate;
    [SerializeField] private RectTransform furnitureEntryContainer;

    [Header("Furniture Detail Display UI Elements")]
    [SerializeField] private FurnitureDetailController furnitureDetailController;



    private FurnitureItem selectedFurnitureItem;
    private FurnitureItemListEntry selectedEntry;

    private List<GameObject> furnitureEntryRefs = new List<GameObject>();
    

    private Dictionary<int, FurnitureItem> furnitureEntryIndexDict = new Dictionary<int, FurnitureItem>();


    public override void ControlledOpen()
    {
        RefreshFurnitureListUI();
        base.ControlledOpen();
    }

    private void RefreshFurnitureListUI()
    {
        ClearInstantiatedUI();

        float width = furnitureEntryContainer.sizeDelta.x;
        float entryHeight = 100;
        furnitureEntryContainer.sizeDelta = new Vector2(width, entryHeight * furnitureDatabase.furnitureList.Count);

        List<FurnitureItem> undiscoveredItems = new List<FurnitureItem>();

        foreach (FurnitureItem item in furnitureDatabase.furnitureList)
        {
            if (item.parentItems == null || item.parentItems.Count == 0 || RecordKeeper.IsCompleted(item))
            {
                InitNewEntry(item, false);
            }
            else
            {
                undiscoveredItems.Add(item);
            }
        }

        // Initialize a blank entry per undiscovered item
        foreach (FurnitureItem item in undiscoveredItems)
        {
            InitNewEntry(item, true);
        }
    }

    private void InitNewEntry(FurnitureItem item, bool undiscovered)
    {
        GameObject newEntry = GameObject.Instantiate(furnitureEntryTemplate, furnitureEntryContainer);
        newEntry.SetActive(true);

        if (newEntry.TryGetComponent<FurnitureItemListEntry>(out FurnitureItemListEntry entryController))
        {
            if (!undiscovered)
                entryController.InitializeDetails(item.displayName, item.sprite);

            entryController.GetButtonComponent().onClick.AddListener(() => {
                OnPickedEntry(entryController, item, undiscovered);
            });

            // Restore last open furniture screen
            if (selectedFurnitureItem != null && selectedFurnitureItem.Equals(item))
            {
                OnPickedEntry(entryController, item, undiscovered);
            }
            // Select the first item if there is no selection to restore
            else if (selectedFurnitureItem == null)
            {
                OnPickedEntry(entryController, item, undiscovered);
            }
        }
        else { Debug.LogWarning("Furniture Entry didn't have a controller! Thumbnail and Name not initialized."); }

        furnitureEntryRefs.Add(newEntry);
    }


    private void ClearInstantiatedUI()
    {
        furnitureDetailController.ClearInstantiatedRecipes();

        foreach (GameObject entry in furnitureEntryRefs)
        {
            Destroy(entry);
        }
        furnitureEntryRefs.Clear();

        furnitureEntryIndexDict.Clear();
    }


    private void OnPickedEntry(FurnitureItemListEntry entryController, FurnitureItem item, bool undiscovered = false)
    {
        if (selectedEntry != null)
        {
            selectedEntry.SetPressedState(false);
        }

        selectedEntry = entryController;
        selectedEntry.SetPressedState(true);

        selectedFurnitureItem = item;

        if (undiscovered)
        {
            furnitureDetailController.DisplayUndiscoveredItemPanel();
        }
        else
        {
            furnitureDetailController.DisplayItemDetails(item);
        }
    }

    
}
