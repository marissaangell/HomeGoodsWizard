using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


[System.Serializable]
public class SaveData
{
    public double Version = 1.0;

    public int SaveSlot;
    public SaveState SaveState;

    public SaveData(SaveState saveState, int saveSlot)
    {
        this.SaveState = saveState;
        this.SaveSlot = saveSlot;
    }
}


[System.Serializable]
public class SaveState
{
    public int SceneIdx;

    public string[] CompletedRecipes;
    public string[] CompletedFurniture;
    public string[] SubmittedFurniture;

    public SaveState(List<string> completedRecipes,
                     List<string> completedFurniture,
                     List<string> submittedFurniture)
    {
        SceneIdx = SceneManager.GetActiveScene().buildIndex;

        CompletedRecipes = completedRecipes.ToArray();
        CompletedFurniture = completedFurniture.ToArray();
        SubmittedFurniture = submittedFurniture.ToArray();
    }
}


/*
* Data Structure Conversion Helper Methods
*/
static class SaveHelpers
{
    public static float[] VectorToFloatArr(Vector3 vec)
    {
        float[] arr = new float[3];
        arr[0] = vec.x;
        arr[1] = vec.y;
        arr[2] = vec.z;
        return arr;
    }

    public static float[][] VectorArrToFloat2D(Vector3[] vecArr)
    {
        float[][] floatArr = new float[vecArr.Length][];
        for (int i = 0; i < vecArr.Length; i++)
        {
            floatArr[i] = VectorToFloatArr(vecArr[i]);
        }
        return floatArr;
    }


}

