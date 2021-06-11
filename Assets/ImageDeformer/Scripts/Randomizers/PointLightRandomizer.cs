using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PointLightRandomizer : Randomizer
{
    public BoxCollider lightRegion;
    public Vector2 lightCount = new Vector2(0, 5);
    public Vector2 intensityRange = new Vector2(10, 150);
    public Vector2 temperatureRange = new Vector2(1500, 20000);
    public float probOfWhite = 0.5f;

    public override void Randomize()
    {
        ClearChildren();
        float count = Random.Range(lightCount.x, lightCount.y);
        for (int i = 0; i < count; ++i)
        {
            Vector3 startPoint = ModelsRandomizer.RandomPointInBounds(lightRegion.bounds);
            GameObject lightObj = new GameObject("Light");
            lightObj.transform.parent = transform;
            lightObj.transform.rotation = Random.rotation;
            lightObj.transform.position = startPoint;
            HDAdditionalLightData light = lightObj.AddHDLight(HDLightTypeAndShape.Point);
            light.intensity = Random.Range(intensityRange.x, intensityRange.y);
            if (Random.value <= probOfWhite)
            {
                float colorTemp = Random.Range(temperatureRange.x, temperatureRange.y);
                light.SetColor(Color.white, colorTemp);
            }
        }
    }

    public override void Default()
    {
        ClearChildren();
    }
}
