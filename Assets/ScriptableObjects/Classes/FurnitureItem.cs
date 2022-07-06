using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/FurnitureItem")]
public class FurnitureItem : ScriptableObject
{
    [UniqueIdentifier]
    [SerializeField]
    private string uniqueID;
    public string ID => uniqueID;


    public string displayName;

    public Sprite sprite;

    public GameObject prefab;

    public List<Recipe> recipes;

    public List<FurnitureItem> parentItems; //current use: tells RecipeUI whether the item is a starting item or an evolution

    public bool submittable = false;


    private void OnValidate()
    {
        List<Recipe> toDelete = new List<Recipe>();

        foreach (Recipe recipe in recipes)
        {
            if (recipe.startingObject.ID != this.ID)
                toDelete.Add(recipe);
        }

        foreach (Recipe recipe in toDelete)
        {
            Debug.LogWarning("[" + this.displayName + "] removed non-matching recipe: " + recipe.name);
            recipes.Remove(recipe);
        }
    }
}
