using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScenePreparation), true)]
public class ScemePreparationEditor : Editor
{
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Scene"))
        {
            (target as ScenePreparation).GenerateScene();
        }

        if (GUILayout.Button("Drop Legos"))
        {
            ModelsRandomizer[] randomizers = Object.FindObjectsOfType<ModelsRandomizer>();
            foreach (ModelsRandomizer randomizer in randomizers)
            {
                randomizer.Randomize();
            }
        }
    }
}
