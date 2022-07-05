using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class RecipeNoteController : DetailsSnippet
{
    [SerializeField] private TextMeshProUGUI recipeDetailTextbox;

    private int numLinesInDetailTextbox = 5;

    public void SetHintText(string text)
    {
        recipeDetailTextbox.text = text;
    }

    public void SetRecipeSteps(List<RecipeStep> steps)
    {
        StringBuilder sb = new StringBuilder();
        
        if (steps.Count <= numLinesInDetailTextbox)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                RecipeStep step = steps[i];

                sb.Append((i+1) + ". ");
                sb.Append(step.tool);
                sb.Append(" -> ");
                sb.Append(step.location);

                if (i != steps.Count - 1)
                    sb.Append("\n");
            }
        }
        else
        {
            for (int i = 0; i < steps.Count; i++)
            {
                RecipeStep step = steps[i];

                sb.Append(step.tool);
                sb.Append(" -> ");
                sb.Append(step.location);

                if (i != steps.Count - 1)
                    sb.Append(", ");
            }
        }
        

        recipeDetailTextbox.text = sb.ToString();
    }
}
