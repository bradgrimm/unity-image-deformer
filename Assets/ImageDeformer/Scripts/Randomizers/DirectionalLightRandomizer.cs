using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DirectionalLightRandomizer : Randomizer
{
    public Vector2 intensityRange = new Vector2(1, 15);
    public Vector2 temperatureRange = new Vector2(1500, 20000);
    public float probOfWhite = 0.75f;
    public float defaultIntensity = 10.0f;

    public override void Randomize()
    {
        ChangeRotation();
        ChangeTemperature(Random.value <= probOfWhite);
        ChangeIntensity(false);
    }

    public override void Default()
    {
        ChangeRotation();
        ChangeIntensity(true);
        ChangeTemperature(true);
    }

    private void ChangeRotation()
    {
        Light lightObj = GetComponent<Light>();
        lightObj.transform.rotation = Random.rotation;
        if (Vector3.Dot(lightObj.transform.forward, Vector3.up) > 0)
            lightObj.transform.RotateAround(lightObj.transform.position, Vector3.right, 180f);
    }

    private void ChangeTemperature(bool isWhite)
    {
        HDAdditionalLightData light = GetComponent<HDAdditionalLightData>();
        float colorTemp = isWhite ? 6607 : Random.Range(temperatureRange.x, temperatureRange.y);
        light.SetColor(Color.white, colorTemp);
    }

    private void ChangeIntensity(bool isDefault)
    {
        HDAdditionalLightData light = GetComponent<HDAdditionalLightData>();
        light.intensity = isDefault ? defaultIntensity : Random.Range(intensityRange.x, intensityRange.y);
    }
}
