using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecordKeeper
{
    public static List<FurnitureItem> submittedFurniture = new List<FurnitureItem>();

    public static List<FurnitureItem> completedFurniture = new List<FurnitureItem>();

    // Todo: Determine how this can be used, or delete it
    public static List<Recipe> completedRecipes = new List<Recipe>();

    public static void LogCompleted(Recipe recipe)
    {
        if (!completedRecipes.Contains(recipe))
        {
            completedRecipes.Add(recipe);
        }
    }

    public static void LogCompleted(FurnitureItem furnitureItem)
    {
        if (!completedFurniture.Contains(furnitureItem))
        {
            completedFurniture.Add(furnitureItem);
        }
    }

    public static bool Completed(Recipe recipe)
    {
        return completedRecipes.Contains(recipe);
    }

    public static bool Completed(FurnitureItem furnitureItem)
    {
        return completedFurniture.Contains(furnitureItem);
    }

    public static void ResetState()
    {
        submittedFurniture.Clear();
        completedFurniture.Clear();
        completedRecipes.Clear();
    }
}

public struct FurnitureRecord
{
    private bool furnitureHasBeenCrafted;
    private Dictionary<int, bool> recipeStatuses;
}