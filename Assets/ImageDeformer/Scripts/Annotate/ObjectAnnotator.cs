using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnnotator : MonoBehaviour
{
    public bool showBounds = false;
    public Color color;

    public void OnGUI()
    {
        if (showBounds) 
        {
            Rect rect = CalculateBoundingBox();
            if (rect.height == 0 || rect.width == 0) return;
            UnityEditor.Handles.BeginGUI();
            UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.red);
            UnityEditor.Handles.EndGUI();
        }
    }

    public Rect CalculateBoundingBox()
    {
        Camera camera = Camera.main;
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        return CalculateBoundingBoxForCamera(camera, mesh);
    }

    public Rect CalculateBoundingBoxForCamera(Camera camera, Mesh mesh)
    {
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        Bounds bounds = collider.bounds;
        Vector3 center = bounds.center;

        if (camera.WorldToScreenPoint(center).z < 0)
        {
            //The object is behind us.
            return Rect.zero;
        }

        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = camera.WorldToScreenPoint(transform.TransformPoint(vertices[i]));
            vertices[i].y = Screen.height - vertices[i].y;
        }

        Vector3 min = vertices[0];
        Vector3 max = vertices[0];
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 w = vertices[i];
            min = Vector3.Min(min, w);
            max = Vector3.Max(max, w);
        }

        Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        rect.xMin = Mathf.Clamp(rect.xMin, 0, Screen.width);
        rect.xMax = Mathf.Clamp(rect.xMax, 0, Screen.width);
        rect.yMin = Mathf.Clamp(rect.yMin, 0, Screen.height);
        rect.yMax = Mathf.Clamp(rect.yMax, 0, Screen.height);
        return rect;
    }
}
