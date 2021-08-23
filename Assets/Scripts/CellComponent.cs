using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellComponent : MonoBehaviour
{
    private Image _image;
    public bool ChangedTheColor { get; private set; } = false;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeColor(Color Playercolor)
    {
        if (ChangedTheColor) return;
        
        var color = _image.color;
        color.a = 1f;
        
        _image.color = Playercolor;
        ChangedTheColor = true;
    }
}
