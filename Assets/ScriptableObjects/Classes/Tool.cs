using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/Tool")]
public class Tool : ScriptableObject
{
    public ToolType type;

    public string verbText;

    public Sprite sprite;
}



public enum ToolType
{
    UNDEFINED,
    Grab,
    Knock,
    Erase,
    Shake,
    Pull,
    Polish
}