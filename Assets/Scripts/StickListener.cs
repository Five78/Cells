using UnityEngine;
using UnityEngine.UI;

public class StickListener : MonoBehaviour
{
    [SerializeField] private Vector2 offset;

    private Image image;
    private BoxCollider2D boxCollider2D;
    private GridLayoutGroup layoutGroup;

    private void Start()
    {
        image = GetComponent<Image>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        layoutGroup = GetComponentInParent<GridLayoutGroup>();

        NormalizeBoxCollider();
    }

    //По палочкам трудно попасть, поэтому расширяем их коллайдер
    private void NormalizeBoxCollider()
    {
        boxCollider2D.size = layoutGroup.cellSize + offset;
    }

    private void OnMouseUp()
    {
        PaintOver();
    }

    private void PaintOver()
    {
        if (image.color != Color.red)
            image.color = Color.red;
        else
            image.color = Color.green;
    }
}
