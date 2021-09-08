using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocalizationDef", fileName = "LocalizationDef")]
public class LocaleDef : ScriptableObject
{
    [SerializeField] private string _url;
    [SerializeField] private List<LocaleItem> _localeItmes;

    private UnityWebRequest _request;

    public Dictionary<string, string> GetData()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var localeItem in _localeItmes)
        {
            dictionary.Add(localeItem.Key, localeItem.Value);
        }
        return dictionary;
    }

    [ContextMenu("Update locales")]
    public void LoadLocale()
    {
        if (_request != null) return;

        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDateLoaded;
    }

    private void OnDateLoaded(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            var rows = _request.downloadHandler.text.Split('\n');
            _localeItmes.Clear();
            foreach (var row in rows)
            {
                AddLocaleItem(row);
            }
        }
    }

    private void AddLocaleItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localeItmes.Add(new LocaleItem { Key = parts[0], Value = parts[1] });
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't parse row: {row}.\n{e}");
        }
    }

    [Serializable]
    private class LocaleItem
    {
        public string Key;
        public string Value;
    }
}
