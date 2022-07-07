using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "CreateCustom/Recipe")]
public class Recipe : ScriptableObject
{
    [UniqueIdentifier]
    [SerializeField]
    private string uniqueID;
    public string ID => uniqueID;


    public FurnitureItem startingObject;

    [Tooltip("What the <c>startingObject</c> turns into if the recipe reqirements are fulfilled")]
    public FurnitureItem resultObject;

    public List<RecipeStep> recipeSteps;

    [Tooltip("True if recipe steps are unordered, false if step order matters")]
    public bool unorderedSteps;

    [TextArea]
    public string recipeHintText;

    // TODO: Figure out how to make the below consistent + make sense
    /*[Tooltip("If true, then interactions that don't share a type with those in the recipe will be ignored.")]
    public bool allowSuperfluousInteractions;*/
    
    /// <summary>
    /// Returns true if the sequence defined by inputSteps fulfills the recipe
    /// </summary>
    /*public bool Matches(List<RecipeStep> inputSteps)
    {
        if (inputSteps == null || recipeSteps == null)
        {
            Debug.LogWarning("Found NULL when trying to compare recipe (" + recipeSteps + ") to input step sequence (" + inputSteps + ")");
            return false;
        }

        if (unorderedSteps)
        {
            // return true if the input sequence has the same quantity of each tool as the recipe
            bool isEqual = (recipeSteps.OrderBy(x => x.tool).ThenBy(x => x.location)).SequenceEqual(inputSteps.OrderBy(x => x.tool).ThenBy(x => x.location));
            return isEqual;
        }
        else
        {
            // return true if the input sequence exactly matches the recipe sequence
            bool isEqual = recipeSteps.SequenceEqual(inputSteps);
            return isEqual;
        }
    }*/

    //https://stackoverflow.com/questions/332973/check-whether-an-array-is-a-subset-of-another
    public MatchStatus CheckForMatch(List<RecipeStep> inputSteps)
    {
        MatchStatus status = MatchStatus.Failed;

        if (inputSteps == null || recipeSteps == null)
        {
            Debug.LogWarning("Found NULL when trying to compare recipe (" + recipeSteps + ") to input step sequence (" + inputSteps + ")");
            return MatchStatus.Failed;
        }

        // Compare # of steps
        int currNumSteps = inputSteps.Count;
        if (currNumSteps > recipeSteps.Count) { return MatchStatus.Failed; }

        // Compare contents of steps (based on whether the recipe is ordered/unordered)
        bool isSubset;
        if (unorderedSteps)
        {   //unordered steps => check if sublist
            //https://stackoverflow.com/questions/29359391/c-sharp-linq-method-to-determine-if-array-is-subset-of-another-including-duplic

            var l1 = recipeSteps.ToLookup(x => x);
            var l2 = inputSteps.ToLookup(x => x);
            isSubset = l2.All(xs => xs.Count() <= l1[xs.Key].Count());
        }
        else
        {   //ordered steps => check if subsequence
            isSubset = inputSteps.SequenceEqual(recipeSteps.GetRange(0, currNumSteps));
        }

        // Determine match status
        if (!isSubset)                              { status = MatchStatus.Failed; }
        else if (currNumSteps == recipeSteps.Count) { status = MatchStatus.ExactMatch; }
        else if (currNumSteps < recipeSteps.Count)  { status = MatchStatus.PartialMatch; }

        Debug.Log("[" + this.name + "]: " + status.ToString());

        return status;
    }
}

[System.Serializable]
public struct RecipeStep
{
    [Tooltip("ToolType of the interaction required for this step")]
    public ToolType tool;

    [Tooltip("Name (string) of the FurnitureComponent where the tool must be applied")]
    public string location;

    public RecipeStep(ToolType _tool, string _location)
    {
        tool = _tool;
        location = _location;
    }
}

public enum MatchStatus
{
    PartialMatch,
    ExactMatch,
    Failed
}
