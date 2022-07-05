using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/Database/RecipeDatabase")]
[System.Serializable]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField] public List<Recipe> recipeList;
}
