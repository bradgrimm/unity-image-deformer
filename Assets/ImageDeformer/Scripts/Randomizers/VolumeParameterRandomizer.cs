using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class VolumeParameterRandomizer<T> : Randomizer where T : VolumeComponent
{
    Volume globalVolume;

    public override void Default()
    {
        T t = GetVolumeParameter();
        t.active = false;
    }

    public T GetVolumeParameter()
    {
        if (globalVolume == null)
            globalVolume = GetComponent<Volume>();

        List<VolumeComponent> components = globalVolume.profile.components;
        for (int i = 0; i < components.Count; i++)
        {
            string name = components[i].name;
            string expected = typeof(T).Name;
            if(name.Contains(expected))
                return (T) components[i];
        }
        return default(T);
    }
}
