using UnityEngine;
using UnityEngine.UI;

public class StickListener : MonoBehaviour
{
    private Image _image;
    private GameController _controller;
    private GameControllerAI _controllerAI;
    private StickSound _sound;

    public bool Standing { get; set; } = false;

    private void Start()
    {
        _image = GetComponent<Image>();
        _sound = GetComponent<StickSound>();
        _controller = FindObjectOfType<GameController>();        

        if (_controller == null)
            _controllerAI = FindObjectOfType<GameControllerAI>();
    }
    
    public void OnClick()
    {
        var color = _image.color;
        color.a = 1f;

        if (_controller != null)
            _image.color = _controller.WhoseMoveNow();
        else
            _image.color = _controllerAI.WhoseMoveNow();
        
        var button = GetComponent<Button>();
        button.interactable = false;
        
        Standing = true;
        _sound.OnPointerClick();

        if (_controller != null)
            _controller.MoveIsMade();
        else
            _controllerAI.MoveIsMade();
    }

}
