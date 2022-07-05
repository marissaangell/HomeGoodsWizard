using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBuildSettings : MonoBehaviour
{
    [SerializeField] private GameObject[] webglHiddenItems;

    private void Awake()
    {

#if UNITY_WEBGL
        foreach(GameObject gameObject in webglHiddenItems)
        {
            gameObject.SetActive(false);
        }
#endif

    }
}
