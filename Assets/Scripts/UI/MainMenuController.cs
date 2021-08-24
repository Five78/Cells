using UnityEngine;
using Object = UnityEngine.Object;

public class MainMenuController : MonoBehaviour
{
    private void CreateWindow(string path)
    {
        var window = Resources.Load<GameObject>(path);
        var canvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();

        Object.Instantiate(window, canvas.transform);
    }
    
    public void OnCreatingGame()
    {
        CreateWindow("CreatingGame");
    }

    public void OnSettingWindow()
    {
        CreateWindow("SettingWindow");
    }

    public void OnRulesWindow()
    {
        CreateWindow("RulesWindow");
    }

    public void OnShopWindow()
    {
        CreateWindow("ShopWindow");
    }


}
