using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sinbad;
using System.IO;

public class ScenePreparation : MonoBehaviour
{
    private Randomizer[] randomizers = new Randomizer[0];

    public void Start()
    {
        Time.timeScale = 5;
    }

    void InitRandomizers()
    {
        if (randomizers.Length == 0)
            randomizers = Resources.FindObjectsOfTypeAll<Randomizer>();
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

    public void SaveSceneToDisk(string subDir="")
    {
        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        ObjectAnnotator[] annotators = Resources.FindObjectsOfTypeAll<ObjectAnnotator>();
        List<YoloObject> visibleObjects = new List<YoloObject>();
        foreach (ObjectAnnotator annotator in annotators)
        {
            Rect rect = annotator.CalculateBoundingBox();
            if (rect.width > 0 && rect.height > 0)
            {
                Renderer renderer = annotator.gameObject.GetComponentInChildren<Renderer>();
                Color color = annotator.color;

                YoloObject obj = new YoloObject();
                string name = annotator.name == "default"
                    ? annotator.transform.parent.name
                    : annotator.name;
                obj.id = name;
                obj.x = rect.x / Screen.width;
                obj.y = rect.y / Screen.height;
                obj.w = rect.width / Screen.width;
                obj.h = rect.height / Screen.height;
                obj.r = color.r;
                obj.g = color.g;
                obj.b = color.b;
                obj.a = color.a;
                visibleObjects.Add(obj);
            }
        }
        string path = "Output/" + subDir + "/";
        Directory.CreateDirectory(path);
        string guid = (string) System.Guid.NewGuid().ToString();
        CsvUtil.SaveObjects<YoloObject>(visibleObjects, path + guid + ".csv");
        ScreenCapture.CaptureScreenshot(path + guid + ".png");
    }
}
