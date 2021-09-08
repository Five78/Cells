using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocaleItemWidget : MonoBehaviour
{
    private Dropdown _dropdown;
    private Dictionary<string, int> _locales = new Dictionary<string, int>
    {
        { "ru", 0},
        { "en", 1},
        { "de", 2},
        { "fr", 3},
        { "es", 4},
        { "zh", 5}
    };

    private void Start()
    {
        _dropdown = GetComponent<Dropdown>();
        var language = LocalizationManager.I.LocaleKey;     
        _dropdown.value = _locales[language];      
    }

    public void OnValueChanged()
    {        
        string key = _locales.Where(x => x.Value == _dropdown.value).FirstOrDefault().Key;
        LocalizationManager.I.SetLocale(key);
    }
}