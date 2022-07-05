using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/FurnitureItem")]
public class FurnitureItem : ScriptableObject
{
    public string displayName;

    public Sprite sprite;

    public GameObject prefab;

    public List<Recipe> recipes;

    public List<FurnitureItem> parentItems; //current use: tells RecipeUI whether the item is a starting item or an evolution

    public bool submittable = false;
}
