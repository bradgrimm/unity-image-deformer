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
            ablationManager.Step();

        if (GUILayout.Button("Run"))
            ablationManager.Run();

        if (GUILayout.Button("Stop"))
            ablationManager.Stop();
    }
}
