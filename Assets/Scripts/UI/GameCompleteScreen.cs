using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameCompleteScreen : PopupMenu
{
    [Header("Game Complete Screen")]
    [SerializeField] private TextMeshProUGUI performanceStatTextField;
    [SerializeField] private TextMeshProUGUI completionStatTextField;
    [SerializeField] private TextMeshProUGUI clientNoteTextField;

    [SerializeField] private FurnitureDatabase furnitureDatabase;

    [SerializeField] private List<CompletionText> completionTextStructs = new List<CompletionText>();

    public override void ControlledOpen()
    {
        PopulateTextValues();
        base.ControlledOpen();
    }

    public void PopulateTextValues()
    {
        int submittedFurnitureCount = RecordKeeper.submittedFurniture.Count;
        int totalFurnitureCount = furnitureDatabase.GetTotalFurnitureCount();

        float completionPercentage = submittedFurnitureCount / (1.0f * totalFurnitureCount);
        CompletionText completionText = GetCompletionText(completionPercentage);

        SetPerformanceText(completionText.performanceRating);
        SetCompletionText(submittedFurnitureCount, totalFurnitureCount, completionPercentage);
        SetClientNoteText(completionText.clientNote);
    }

    private CompletionText GetCompletionText(float completionPercentage)
    {
        CompletionText bestFit = completionTextStructs[0];
        foreach (CompletionText currStruct in completionTextStructs)
        {
            if (currStruct.requiredCompletion > bestFit.requiredCompletion 
                && completionPercentage >= currStruct.requiredCompletion)
            {
                bestFit = currStruct;
            }
        }
        return bestFit;
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void SetPerformanceText(string text)
    {
        performanceStatTextField.text = text;
    }

    private void SetCompletionText(int numerator, int denominator, float percentage)
    {
        int percentInt = (int)(percentage * 100f);

        completionStatTextField.text = numerator + " / " + denominator + " (" + percentInt + "%)";
    }

    private void SetClientNoteText(string text)
    {
        clientNoteTextField.text = text;
    }
}

[System.Serializable]
public struct CompletionText
{
    [SerializeField][Range(0f, 1f)] public float requiredCompletion;
    [SerializeField] public string performanceRating;
    [SerializeField][TextArea] public string clientNote;
}
