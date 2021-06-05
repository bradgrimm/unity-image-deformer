using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Randomizer), true)]
public class RandomizerEditor : Editor
{
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Trigger Randomization"))
        {
            (target as Randomizer).Randomize();
        }
    }
}
