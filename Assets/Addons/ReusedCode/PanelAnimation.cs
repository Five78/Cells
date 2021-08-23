using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelAnimation : MonoBehaviour
{
    //Анимация для открытия/закрытия панелей в UI

    //Корутина, которая хранит анимацию
    private ExtendedCoroutine changeVisibility;
    //Переменная нужна для AndroidbackButton
    public static bool IsAnyAnimationRunning { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float speed;

    private void Start()
    {
        changeVisibility = new ExtendedCoroutine(this, ChangeVisibility);
    }

    public void ChangeAnimationState()
    {
        changeVisibility.Start();
    }

    private IEnumerator ChangeVisibility()
    {
        IsAnyAnimationRunning = true;

        bool isVisible = canvasGroup.alpha == 1f;
        float target = isVisible ? 0f : 1f;

        if (!isVisible && (Vector2)transform.localScale == Vector2.one)
            transform.localScale = new Vector2(1.2f, 1.2f);
        Vector2 targetScale = isVisible ? transform.localScale * 1.2f : transform.localScale / 1.2f;

        while (canvasGroup.alpha != target)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, speed * Time.deltaTime);
            transform.localScale = Vector2.MoveTowards(transform.localScale, targetScale, speed / 2 * Time.deltaTime);
            
            yield return null;
        }

        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;

        IsAnyAnimationRunning = false;
    }
}
