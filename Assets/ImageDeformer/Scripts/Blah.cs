using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Blah : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {
        string[] guids1 = AssetDatabase.FindAssets("", new[] {"Assets/Textures/Lens Dirt"});

        foreach (string guid1 in guids1)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
