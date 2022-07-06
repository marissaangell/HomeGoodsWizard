using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureDetailController : DetailsSnippet
{
    [Header("Panels")]
    [SerializeField] private GameObject furnitureDetailPanel;
    [SerializeField] private GameObject undiscoveredItemPanel;

    [Header("Recipe Details")]
    [SerializeField] private GameObject recipeNoteTemplate;
    [SerializeField] private RectTransform recipeNoteContainer;

    [Header("Status Image")]
    [SerializeField] private Image statusImageRef;
    [SerializeField] private Sprite notSubmittedSprite;
    [SerializeField] private Sprite submittedSprite;

    private List<GameObject> recipeNoteRefs = new List<GameObject>();

    public override void InitializeDetails(string name, Sprite thumbnailSprite)
    {
        base.InitializeDetails(name, thumbnailSprite);

        furnitureDetailPanel.SetActive(true);
        undiscoveredItemPanel.SetActive(false);
    }

    public void DisplayUndiscoveredItemPanel()
    {
        furnitureDetailPanel.SetActive(false);
        undiscoveredItemPanel.SetActive(true);

        statusImageRef.gameObject.SetActive(false);
    }

    public void DisplayItemDetails(FurnitureItem item)
    {
        ClearInstantiatedRecipes();

        foreach (Recipe recipe in item.recipes)
        {
            CreateRecipeNote(recipe);
        }
        
        if (item.submittable)
        {
            statusImageRef.gameObject.SetActive(true);
            if (RecordKeeper.IsSubmitted(item))
            {
                statusImageRef.sprite = submittedSprite;
                statusImageRef.color = Color.green;
            }
            else
            {
                statusImageRef.sprite = notSubmittedSprite;
                statusImageRef.color = Color.red;
            }
        }
        else
        {
            statusImageRef.gameObject.SetActive(false);
        }

        InitializeDetails(item.displayName, item.sprite);
    }

    private void CreateRecipeNote(Recipe recipe)
    {
        GameObject newNote = GameObject.Instantiate(recipeNoteTemplate, recipeNoteContainer);
        newNote.SetActive(true);

        if (newNote.TryGetComponent<RecipeNoteController>(out RecipeNoteController controller))
        {
            if (RecordKeeper.IsCompleted(recipe))
            {
                controller.SetName(recipe.resultObject.displayName);
                controller.SetThumbnail(recipe.resultObject.sprite);
                controller.SetRecipeSteps(recipe.recipeSteps);
            }
            else if (recipe.recipeHintText != string.Empty)
            {
                // Recipe is not completed, so set hint text (if it exists)
                controller.SetHintText(recipe.recipeHintText);
            }
        }
        else { Debug.LogWarning("Furniture Entry didn't have a controller! Thumbnail and Name not initialized."); }

        recipeNoteRefs.Add(newNote);
    }

    private void OnDestroy()
    {
        ClearInstantiatedRecipes();
    }

    public void ClearInstantiatedRecipes()
    {
        foreach (GameObject note in recipeNoteRefs)
        {
            Destroy(note);
        }
        recipeNoteRefs.Clear();
    }
}
