using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AblationManager), true)]
public class AblationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AblationManager ablationManager = target as AblationManager;
        if (GUILayout.Button("Step"))
            ablationManager.isSingleShot = true;

        if (GUILayout.Button("Run"))
            ablationManager.isRunning = true;

        if (GUILayout.Button("Stop"))
            ablationManager.isRunning = false;
    }
}
