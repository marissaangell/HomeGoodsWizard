using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class FurnitureController : MonoBehaviour
{
    [Header("ScriptableObject")]
    [Tooltip("The ScriptableObject corresponding to the furniture prefab this script is sitting on")]
    [SerializeField] private FurnitureItem thisItem;

    [Header("Targeting")]
    [SerializeField] private bool finalStageFurniture = false;
    [SerializeField] public Outline wholeObjectOutline;
    //[Tooltip("Tools that should target the whole object instead of individual components - Erase added in Start()")]
    //[SerializeField] public List<ToolType> wholeObjectTargetingTools;
    private List<ToolType> wholeObjectTargetingTools = new List<ToolType> { ToolType.Erase };

    [Header("Particles")]
    [SerializeField] public ParticleSystem burstSystem;
    [SerializeField] public ParticleSystem continuousSystem;

    [Header("Sounds")]
    [SerializeField] private List<InteractSoundConfiguration> soundConfigurations;

    private List<RecipeStep> interactionHistory = new List<RecipeStep>();

    public FurnitureItem ObjectReference => thisItem;
    public string ObjectName => thisItem.displayName;
    public bool Submittable => thisItem.submittable;

    public bool WaitingToEvolve { get; private set; } = false;

    private void Awake()
    {
        if (thisItem == null)
        {
            Debug.LogWarning("ScriptableObject + [" + gameObject.name + "] does not have its furniture item reference set!");
        }
    }

    private void OnEnable()
    {
        //GlobalEventHandler.EvolveFurnitureEvent += EvolveFurniture;
    }

    private void OnDisable()
    {
        //GlobalEventHandler.EvolveFurnitureEvent -= EvolveFurniture;
    }

    public delegate void ReadyToEvolveDelegate();
    public event ReadyToEvolveDelegate ReadyToEvolve;


    void Start()
    {
        if (wholeObjectOutline != null)
        {
            wholeObjectOutline.enabled = false;
        } else if (!finalStageFurniture){ Debug.LogWarning("[" + gameObject.name + "] has no whole object outline!"); }
    }

    public bool ShouldTargetComponent(ToolType tool)
    {
        if (wholeObjectTargetingTools != null && wholeObjectTargetingTools.Contains(tool))
        {
            if (wholeObjectOutline != null)
                wholeObjectOutline.enabled = true;

            return false;
        }
        else
        {
            return true;
        }
    }

    public void LogUntarget()
    {
        if (wholeObjectOutline != null)
        {
            wholeObjectOutline.enabled = false;
        }
    }

    public void LogInteraction(ToolType tool, FurnitureComponent component)
    {
        if (tool == ToolType.Erase)
        {
            ClearInteractionHistory();
        }
        else if (WaitingToEvolve)
        {
            return; //reject all interactions if it's waiting to evolve
        }
        else
        {
            interactionHistory.Add(new RecipeStep(tool, component.LocationName));
        }

        // Play interaction sound
        PlayInteractSound(tool, component.LocationName);

        // Check for recipe validation after each interaction
        // TODO: Switch from looping over all recipes to keeping an in-progress recipe list
        if (ValidateRecipeSteps(out Recipe completedRecipe))
        {
            Debug.Log("Recipe fulfilled for [" + gameObject.name + "]: " + DebugPrintInteractionHistory(false));
            RecordKeeper.LogCompleted(completedRecipe);

            burstSystem.Play();
            WaitingToEvolve = true;

            ReadyToEvolve.Invoke();
        }
    }

    public void ClearInteractionHistory()
    {
        WaitingToEvolve = false;
        interactionHistory.Clear();
    }

    /// <summary></summary>
    /// <param name="resultingItem">Will hold null if the method returns false, else if a valid recipe is found will hold Recipe.ResultingObject</param>
    /// <returns></returns>
    public bool ValidateRecipeSteps(out Recipe completedRecipe)
    {
        List<Recipe> validRecipes = thisItem.recipes;

        if (validRecipes != null)
        {
            foreach (Recipe recipe in validRecipes)
            {
                if (recipe.Matches(interactionHistory))
                {
                    completedRecipe = recipe;
                    return true;
                }
            }
        }

        completedRecipe = null;
        return false;
    }

    public void EvolveFurniture(out FurnitureController newFurnitureObject)
    {
        if (ValidateRecipeSteps(out Recipe completedRecipe))
        {
            FurnitureItem resultingItem = completedRecipe.resultObject;
            RecordKeeper.LogCompleted(resultingItem);

            Debug.Log(gameObject.name + " evolving into " + resultingItem.displayName);

            GameObject newFurniture = GameObject.Instantiate(resultingItem.prefab, this.transform.position, this.transform.rotation);
            newFurnitureObject = newFurniture.GetComponent<FurnitureController>();

            Destroy(this.gameObject);
        }
        else
        {
            newFurnitureObject = null;
        }
    }

    private void PlayInteractSound(ToolType tool, string locationName)
    {
        foreach (InteractSoundConfiguration config in soundConfigurations)
        {
            bool toolMatches = config.tool == tool;
            bool locMatches = string.IsNullOrEmpty(config.optionalComponentName) || config.optionalComponentName.Equals(locationName);
            
            if (toolMatches && locMatches)
            {
                SoundManager.Play(config.soundToPlay);
                return;
            }
        }
    }

    private string DebugPrintInteractionHistory(bool logToConsole = true)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < interactionHistory.Count; i++)
        {
            sb.Append("(" + interactionHistory[i].tool + " -> " + interactionHistory[i].location + ")");
            if (i != interactionHistory.Count - 1) { sb.Append("\n"); }
        }

        if (logToConsole)
            Debug.Log("Interaction history for [" + gameObject.name + "]: \n" + sb.ToString());

        return sb.ToString();
    }
}


[System.Serializable]
public struct InteractSoundConfiguration
{
    public ToolType tool;
    public string soundToPlay;
    public string optionalComponentName;
}