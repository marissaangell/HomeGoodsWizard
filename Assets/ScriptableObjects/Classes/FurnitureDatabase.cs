using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(menuName = "CreateCustom/Database/FurnitureDatabase")]
[System.Serializable]
public class FurnitureDatabase : ScriptableObject
{
    [SerializeField] public List<FurnitureItem> furnitureList;

    public Dictionary<string, FurnitureItem> furnitureDict = new Dictionary<string, FurnitureItem>();

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

    //Initialize the furniture dictionary
    private void OnValidate()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--Furniture Dictionary Contents--");
        int count = 0;

        furnitureDict.Clear();

        foreach (FurnitureItem item in furnitureList)
        {
            furnitureDict.Add(item.ID, item);
            sb.AppendLine("[" + count + "] " + item.displayName + ": " + item.ID);
            count++;
        }

        Debug.Log(sb.ToString());
    }

}
