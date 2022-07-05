using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/Database/FurnitureDatabase")]
[System.Serializable]
public class FurnitureDatabase : ScriptableObject
{
    [SerializeField] public List<FurnitureItem> furnitureList;

    public int GetTotalRecipeCount()
    {
        int count = 0;
        foreach (FurnitureItem item in furnitureList)
        {
            if (item.recipes != null)
                count += item.recipes.Count;
        }

        return count;
    }

    public int GetTotalFurnitureCount()
    {
        int count = 0;
        foreach (FurnitureItem item in furnitureList)
        {
            if (item.submittable)
                count += 1;
        }

        return count;
    }
}
