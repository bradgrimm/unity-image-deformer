using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePreparation : MonoBehaviour
{
    private Randomizer[] randomizers = new Randomizer[0];

    void InitRandomizers()
    {
        //if (randomizers.Length == 0)
        //{
        randomizers = Resources.FindObjectsOfTypeAll<Randomizer>();
        //}
    }

    public void GenerateScene()
    {
        RandomizeEverything();
    }

    public void RandomizeEverything()
    {
        InitRandomizers();
        foreach (Randomizer randomizer in randomizers)
        {
            bool enabled = randomizer.triggerEnabled;
            float c = randomizer.triggerChance;
            float v = Random.Range(0.0f, 1.0f);
            if (enabled && c != 0.0f && v <= c)
                randomizer.Randomize();
            else
                randomizer.Default();
        }
    }
}
