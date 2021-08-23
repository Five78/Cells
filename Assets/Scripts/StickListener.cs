using UnityEngine;
using UnityEngine.UI;

public class StickListener : MonoBehaviour
{
    private Image _image;
    private GameController _controller;

    public bool Standing { get; private set; } = false;

    private void Start()
    {
        _image = GetComponent<Image>();
        _controller = FindObjectOfType<GameController>();
    }
    
    public void OnClick()
    {
        var color = _image.color;
        color.a = 1f;

        _image.color = _controller.WhoseMoveNow();
        
        var button = GetComponent<Button>();
        button.interactable = false;
        
        Standing = true;
        
        _controller.MoveIsMade();
    }

}
