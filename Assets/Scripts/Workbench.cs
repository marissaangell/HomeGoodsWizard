using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Workbench : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private BroadcastInteractable resetButton;
    [SerializeField] private BroadcastInteractable evolveButton;
    [SerializeField] private BroadcastInteractable submitButton;

    [Header("Item Spawning")]
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private FurnitureItem startingItem;

    [Header("Nameplate")]
    [SerializeField] private TextMeshPro nameplate;

    public delegate void furnitureSubmittedDelegate(FurnitureItem item);
    public event furnitureSubmittedDelegate furnitureSubmittedEvent;

    private FurnitureController currentFurniture;

    private void OnEnable()
    {
        resetButton.InteractReceivedEvent += HandleReset;
        evolveButton.InteractReceivedEvent += HandleEvolve;
        submitButton.InteractReceivedEvent += HandleSubmit;
    }

    private void OnDisable()
    {
        resetButton.InteractReceivedEvent -= HandleReset;
        evolveButton.InteractReceivedEvent -= HandleEvolve;
        submitButton.InteractReceivedEvent -= HandleSubmit;
    }

    private void Start()
    {
        SpawnStartingItem();    
    }

    private void HandleReset()
    {
        if (currentFurniture != null)
            Destroy(currentFurniture.gameObject);

        SpawnStartingItem();
    }

    private void HandleEvolve()
    {
        currentFurniture.EvolveFurniture(out FurnitureController newFurnitureObject);
        newFurnitureObject.transform.parent = itemSpawnPoint;
        currentFurniture = newFurnitureObject;

        currentFurniture.ReadyToEvolve += HandleReadyToEvolve;

        evolveButton.SetInteractive(false);
        if (newFurnitureObject.Submittable)
        {
            submitButton.SetInteractive(true);
        }
        nameplate.text = currentFurniture.ObjectName;
    }

    private void HandleSubmit()
    {
        if (currentFurniture.Submittable)
        {
            //Invoke furniture submitted event
            if (furnitureSubmittedEvent != null)
                furnitureSubmittedEvent.Invoke(currentFurniture.ObjectReference);

            //Destroy current furniture
            Destroy(currentFurniture.gameObject);
        }

        //Disable all buttons other than reset
        evolveButton.SetInteractive(false);
        submitButton.SetInteractive(false);

        nameplate.text = string.Empty;
    }

    private void SpawnStartingItem()
    {
        GameObject newFurniture = GameObject.Instantiate(startingItem.prefab, itemSpawnPoint);
        newFurniture.transform.parent = itemSpawnPoint;
        currentFurniture = newFurniture.GetComponent<FurnitureController>();

        currentFurniture.ReadyToEvolve += HandleReadyToEvolve;

        resetButton.SetInteractive(true); //should always be true, but just in case
        evolveButton.SetInteractive(false);
        submitButton.SetInteractive(false);

        nameplate.text = currentFurniture.ObjectName;
    }

    private void HandleReadyToEvolve()
    {
        evolveButton.SetInteractive(true);
    }
}
