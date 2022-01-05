using UnityEngine;
using UnityEngine.UI;

public class StickListener : MonoBehaviour
{
    private Image _image;
    private GameController _controller;
    private GameControllerAI _controllerAI;
    private GameControllerMP _controllerMP;
    private StickSound _sound;

    public bool Standing { get; set; } = false;

    private void Start()
    {
        _image = GetComponent<Image>();
        _sound = GetComponent<StickSound>();
        _controller = FindObjectOfType<GameController>();        

        if (_controller == null)
            _controllerAI = FindObjectOfType<GameControllerAI>();

        if (_controllerAI == null)
            _controllerMP = FindObjectOfType<GameControllerMP>();
    }
    
    public virtual void OnClick()
    {
        var color = _image.color;
        color.a = 1f;

        if (_controller != null)
            _image.color = _controller.WhoseMoveNow();
        else if (_controllerAI != null)
            _image.color = _controllerAI.WhoseMoveNow();
        else
            _image.color = _controllerMP.WhoseMoveNow();
        
        var button = GetComponent<Button>();
        button.interactable = false;
        
        Standing = true;
        _sound.OnPointerClick();

        if (_controller != null)
            _controller.MoveIsMade();
        else if (_controllerAI != null)
            _controllerAI.MoveIsMade();
        else
            _controllerMP.MoveIsMade();
    }
}
