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
    private List<Recipe> recipesInProgress;

    public FurnitureItem ObjectReference => thisItem;
    public string ObjectName => thisItem.displayName;
    public bool Submittable => thisItem.submittable;

    public EvolveStatus evolveStatus { get; private set; } = EvolveStatus.InProgress;
    private Recipe completedRecipe;


    private Color PARTICLE_COLOR_DEFAULT = Color.white;
    private Color PARTICLE_COLOR_READY = Color.green;
    private Color PARTICLE_COLOR_FAILED = Color.red;


    private void Awake()
    {
        if (thisItem == null)
        {
            Debug.LogWarning("ScriptableObject + [" + gameObject.name + "] does not have its furniture item reference set!");
        }
    }

    public delegate void ReadyToEvolveDelegate();
    public event ReadyToEvolveDelegate ReadyToEvolve;


    void Start()
    {
        ClearInteractionHistory();

        if (wholeObjectOutline != null)
        {
            wholeObjectOutline.enabled = false;
        } else if (!finalStageFurniture) { Debug.LogWarning("[" + gameObject.name + "] has no whole object outline!"); }
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
        else if (evolveStatus == EvolveStatus.InProgress)
        {
            interactionHistory.Add(new RecipeStep(tool, component.LocationName));

            // Validate updated interaction history against recipes in progress
            ValidateRecipes(); 
        }

        // Play interaction sound
        PlayInteractSound(tool, component.LocationName);

        // Visual feedback that interaction was accepted
        ActivateParticleBurst();
    }

    public void ValidateRecipes()
    {
        if (evolveStatus == EvolveStatus.Ready || evolveStatus == EvolveStatus.Failed) { return; }

        List<Recipe> toDelete = new List<Recipe>();

        foreach (Recipe potentialRecipe in recipesInProgress)
        {
            switch (potentialRecipe.CheckForMatch(interactionHistory))
            {
                case MatchStatus.ExactMatch:
                    HandleCompletedRecipe(potentialRecipe);
                    return; //quit method early if exact match found

                case MatchStatus.Failed:
                    toDelete.Add(potentialRecipe);
                    break;

                default:
                    break;
            }
        }

        foreach (Recipe failedRecipe in toDelete)
        {
            recipesInProgress.Remove(failedRecipe);
        }

        if (recipesInProgress.Count == 0)
        {
            evolveStatus = EvolveStatus.Failed;
        }
    }

    public void ClearInteractionHistory()
    {
        this.evolveStatus = EvolveStatus.InProgress;
        interactionHistory.Clear();
        recipesInProgress = new List<Recipe>(thisItem.recipes);
    }

    public void HandleCompletedRecipe(Recipe completedRecipe)
    {
        Debug.Log("Recipe fulfilled for [" + gameObject.name + "]: \n" + DebugPrintInteractionHistory(false));
        RecordKeeper.LogCompleted(completedRecipe);

        this.evolveStatus = EvolveStatus.Ready;
        this.completedRecipe = completedRecipe;

        ReadyToEvolve.Invoke();
    }

    public void EvolveFurniture(out FurnitureController newFurnitureObject)
    {
        if (completedRecipe != null)
        {
            FurnitureItem resultingItem = completedRecipe.resultObject;
            RecordKeeper.LogCompleted(resultingItem);

            Debug.Log(gameObject.name + " evolving into " + resultingItem.displayName);

            GameObject newFurniture = GameObject.Instantiate(resultingItem.prefab, this.transform.position, this.transform.rotation);
            newFurnitureObject = newFurniture.GetComponent<FurnitureController>();

            SoundManager.Play("MagicMysterious");

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

    private void ActivateParticleBurst()
    {
        var main = burstSystem.main;
        switch (this.evolveStatus)
        {
            case EvolveStatus.Failed:
                main.startColor = PARTICLE_COLOR_FAILED;
                break;
            case EvolveStatus.Ready:
                main.startColor = PARTICLE_COLOR_READY;
                break;
            default:
                main.startColor = PARTICLE_COLOR_DEFAULT;
                break;
        }

        burstSystem.Play();
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

public enum EvolveStatus
{
    InProgress,
    Ready,
    Failed
}