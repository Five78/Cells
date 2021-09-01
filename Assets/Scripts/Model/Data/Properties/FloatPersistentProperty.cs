using System;
using UnityEngine;

[Serializable]
public class FloatPersistentProperty : PrefsPersistentProperty<float>
{
    public FloatPersistentProperty(float defaultValue, string key) : base(defaultValue, key)
    {
        Init();
    }

    protected override void Write(float value)
    {
        PlayerPrefs.SetFloat(Key, value);
        PlayerPrefs.Save();
    }

    protected override float Read(float defaulValue)
    {
        return PlayerPrefs.GetFloat(Key, defaulValue);
    }
}
