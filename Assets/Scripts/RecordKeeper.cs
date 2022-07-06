using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RecordKeeper
{
    public static List<string> submittedFurniture = new List<string>();
    public static List<string> completedFurniture = new List<string>();
    public static List<string> completedRecipes = new List<string>();

    public static void LogCompleted(Recipe recipe)
    {
        if (!completedRecipes.Contains(recipe.ID))
        {
            completedRecipes.Add(recipe.ID);
        }
    }

    public static void LogCompleted(FurnitureItem furnitureItem)
    {
        if (!completedFurniture.Contains(furnitureItem.ID))
        {
            completedFurniture.Add(furnitureItem.ID);
        }
    }

    public static void LogSubmitted(FurnitureItem furnitureItem)
    {
        if (!submittedFurniture.Contains(furnitureItem.ID))
        {
            submittedFurniture.Add(furnitureItem.ID);
        }

        if (!completedFurniture.Contains(furnitureItem.ID))
        {
            completedFurniture.Add(furnitureItem.ID);
            Debug.LogWarning("Furniture submitted, but there was no record of it being completed. Item marked as completed.");
        }
    }

    public static bool IsCompleted(Recipe recipe)
    {
        return completedRecipes.Contains(recipe.ID);
    }

    public static bool IsCompleted(FurnitureItem furnitureItem)
    {
        return completedFurniture.Contains(furnitureItem.ID);
    }

    public static bool IsSubmitted(FurnitureItem furnitureItem)
    {
        return submittedFurniture.Contains(furnitureItem.ID);
    }

    public static void ResetState()
    {
        submittedFurniture.Clear();
        completedFurniture.Clear();
        completedRecipes.Clear();
    }

    public static bool RestoreSavedState(SaveState saveState)
    {
        completedRecipes   = new List<string>(saveState.CompletedRecipes);
        completedFurniture = new List<string>(saveState.CompletedFurniture);
        submittedFurniture = new List<string>(saveState.SubmittedFurniture);

        SceneManager.LoadScene(saveState.SceneIdx);

        return true;
    }

    public static void SaveCurrentGameState()
    {
        SaveState saveState = new SaveState(completedRecipes, completedFurniture, submittedFurniture);
        SaveSystem.SaveGame(saveState);
    }
}

/*public struct FurnitureRecord
{
    private bool furnitureHasBeenCrafted;
    private Dictionary<int, bool> recipeStatuses;
}*/