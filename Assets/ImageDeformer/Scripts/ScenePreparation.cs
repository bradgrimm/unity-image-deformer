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
        Camera camera = Camera.main;
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;
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
                obj.id = name.Replace("(Clone)", "");
                float sw = rect.width / width;
                float sh = rect.height / height;
                obj.x = (rect.x / width) + (sw / 2);
                obj.y = (rect.y / height) + (sh / 2);
                obj.w = sw;
                obj.h = sh;
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
