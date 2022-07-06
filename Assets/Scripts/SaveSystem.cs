using UnityEngine;
using System.IO;
using System.Threading;


//Used Brackey's Save/Load video for reference: https://www.youtube.com/watch?v=XOjd_qU2Ido
public static class SaveSystem
{
    private static readonly string FILE_NAME = "/homeGoodsWizard";
    private static readonly string FILE_EXT = ".json";
    private static string persistantPath;

    private static object threadLockObj = new Object();
    private static int activeSaveSlot = 0;

    private static string GetFilePath(int saveSlot)
    {
        if (persistantPath == null) { persistantPath = Application.persistentDataPath; }
        return persistantPath + FILE_NAME + saveSlot + FILE_EXT;
    }

    public static void SaveGame(SaveState saveState)
    {
        if (persistantPath == null) { persistantPath = Application.persistentDataPath; }
        Debug.Log("SAVING: " + GetFilePath(activeSaveSlot));

        SaveData save = new SaveData(saveState, activeSaveSlot);
        string saveDataJSON = JsonUtility.ToJson(save, true);

        //Parameterized thread docs: https://docs.microsoft.com/en-us/dotnet/api/system.threading.parameterizedthreadstart?view=net-6.0
        Thread saveThread = new Thread(SaveSystem.SaveDataToDisk);
        saveThread.Start(saveDataJSON);
    }

    /*
     * Asynchronously write save data to disk
     * Deadlock/Race Condition docs referenced:
     * - https://docs.microsoft.com/en-us/dotnet/standard/threading/managed-threading-best-practices
     * - https://docs.microsoft.com/en-us/dotnet/api/system.threading.monitor.tryenter?view=net-6.0
     * Another option if wanting to change: https://docs.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=net-6.0
     */
    private static void SaveDataToDisk(object saveDataJSON)
    {
        bool lockTaken = false;

        try
        {
            Monitor.TryEnter(threadLockObj, System.TimeSpan.FromMilliseconds(500), ref lockTaken);
            if (lockTaken)
            {
                // The critical section - write save data to disk
                File.WriteAllText(GetFilePath(activeSaveSlot), (string)saveDataJSON);
            }
            else
            {
                // The lock was not acquired.
            }
        }
        finally
        {
            // Ensure that the lock is released.
            if (lockTaken)
            {
                Monitor.Exit(threadLockObj);
            }
        }

    }

    public static SaveData GetStoredSaveData(int saveSlot)
    {
        string path = GetFilePath(saveSlot);
        if (File.Exists(path))
        {
            //Read save data from file + deserialize
            string fileContents = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(fileContents);

            activeSaveSlot = saveData.SaveSlot;

            return saveData;
        }
        else
        {
            Debug.LogError("Tried to load nonexistent save data from: " + path);
            return null;
        }
    }

    public static bool SaveDataExists(int saveSlot)
    {
        return File.Exists(GetFilePath(saveSlot));
    }

    public static void SaveThenQuit(SaveState saveState)
    {
        SaveData save = new SaveData(saveState, activeSaveSlot);
        string saveDataJSON = JsonUtility.ToJson(save, true);
        File.WriteAllText(GetFilePath(activeSaveSlot), (string)saveDataJSON);

        Application.Quit();
    }

}
