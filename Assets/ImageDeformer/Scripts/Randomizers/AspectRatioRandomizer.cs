using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class AspectRatioRandomizer : Randomizer
{
    // Public variables
    public int minSize = 640;

    // Static variables
    static object gameViewSizesInstance;
    static MethodInfo getGroup;
    static string[] resolutions = new string[] {"3:2", "4:3", "16:9", "16:10", "1:1"};

    static void StaticInit()
    {
        if (getGroup == null)
        {
            var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            getGroup = sizesType.GetMethod("GetGroup");
            gameViewSizesInstance = instanceProp.GetValue(null, null);
        }
    }

    public override void Randomize()
    {
        int resIdx = UnityEngine.Random.Range(0, resolutions.Length);
        bool reverse = UnityEngine.Random.value >= 0.5f;
        string resolution = resolutions[resIdx];
        ChangeAspectRatio(resolution, reverse);
    }

    public override void Default()
    {
        ChangeAspectRatio("1:1", false);
    }

    string ResolutionName(int w, int h)
    {
        return "Randomized (" + w + ", " + h + ")";
    }

    int[] GetSize(string res, bool reverse)
    {
        string[] ratioParts = res.Split(':');
        int w = int.Parse(ratioParts[0]);
        int h = int.Parse(ratioParts[1]);
        float ratio = ((float) w / (float) h);
        w = (int) (minSize * ratio);
        h = minSize;
        return new int[2] {reverse ? w : h, reverse ? h : w};
    }

    void ChangeAspectRatio(string resolution, bool reverse)
    {
        int[] size = GetSize(resolution, reverse);
        string name = ResolutionName(size[0], size[1]);
        int idx = FindSize(GameViewSizeGroupType.Standalone, name);
        if (idx == -1)
        {
            AddCustomSize(name, size[0], size[1]);
            idx = FindSize(GameViewSizeGroupType.Standalone, name);
        }
        idx = idx == -1 ? 0 : idx;
        SetSize(idx);
    }

    static void AddCustomSize(string text, int width, int height)
    {
        var asm = typeof(Editor).Assembly;
        var sizesType = asm.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        var getGroup = sizesType.GetMethod("GetGroup");
        var instance = instanceProp.GetValue(null, null);
        var group = getGroup.Invoke(instance, new object[] { (int)GameViewSizeGroupType.Standalone });
        var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
        var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
        var ctor = gvsType.GetConstructor(new Type[] { gameViewSizeType, typeof(int), typeof(int), typeof(string) });
        var fixedResolution = Enum.Parse(gameViewSizeType, "FixedResolution");

        Debug.Log(fixedResolution);
        Debug.Log(width);
        Debug.Log(height);
        Debug.Log(text);
        var newSize = ctor.Invoke(new object[] { fixedResolution, width, height, text });
        addCustomSize.Invoke(group, new object[] { newSize });
    }

    static void SetSize(int index)
    {
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        selectedSizeIndexProp.SetValue(gvWnd, index, null);
    }
    
    static object GetGroup(GameViewSizeGroupType type)
    {
        StaticInit();
        return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
    }

    static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
    {
        var group = GetGroup(sizeGroupType);
        var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
        for (int i = 0; i < displayTexts.Length; i++)
        {
            string display = displayTexts[i];
            if (display.Contains(text))
                return i;
        }
        return -1;
    }
}
