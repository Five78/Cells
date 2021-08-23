using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellComponent : MonoBehaviour
{
    private Image _image;
    private GameSession _session;

    private void Start()
    {
        _image = GetComponent<Image>();
        _session = FindObjectOfType<GameSession>();
    }

    [ContextMenu("Change!")]
    public void ChangeColor()
    {
        var color = _image.color;
        color.a = 1f;
        _image.color = _session.Player1;
    }
}
