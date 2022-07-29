using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateCustom/Database/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField] public Sound[] sounds;
}
