using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRandomizer : ComponentRandomizer<Camera>
{
    public Transform parentForCreatedObjects;
    public Vector2 yRange = new Vector2(1.0f, 3.0f);
    public Vector2 xzRange = new Vector2(0.0f, 2.0f);

    private Transform lookAt;

    void Update()
    {
        if (lookAt == null)
        {
            PickRandomObjectToLookAt();
        }

        if (lookAt != null)
        {
            Camera.main.transform.LookAt(lookAt);
        }
    }

    public override void Randomize()
    {
        Camera camera = GetComponent();
        PickRandomObjectToLookAt();
        Vector2 xzRandom = Random.insideUnitCircle;
        Vector3 cameraPosition = lookAt.position;
        cameraPosition.y += Random.Range(yRange.x, yRange.y);
        cameraPosition.x += xzRandom.x;
        cameraPosition.z += xzRandom.y;
        camera.transform.position = cameraPosition;
    }

    public void PickRandomObjectToLookAt()
    {
        int childCount = parentForCreatedObjects.childCount;
        Transform child = childCount > 1
            ? parentForCreatedObjects.transform.GetChild(Random.Range(0, childCount))
            : parentForCreatedObjects;
        lookAt = child;
    }
}
