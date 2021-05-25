using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnnotator : MonoBehaviour
{
    public bool showBounds = false;

    public void OnGUI()
    {
        if (showBounds) 
        {
            Camera camera = Camera.main;
            Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
            Rect rect = CalculateMeshBoundingBoxForCamera(camera, mesh);
            if (rect.height == 0 || rect.width == 0) return;
            UnityEditor.Handles.BeginGUI();
            UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.red);
            UnityEditor.Handles.EndGUI();
        }
    }

    public Rect CalculateMeshBoundingBoxForCamera(Camera camera, Mesh mesh)
    {
        Bounds bounds = gameObject.GetComponent<MeshCollider>().bounds;
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
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }
}
