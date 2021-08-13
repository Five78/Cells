using System;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{
    //Скрипт реализует кнопку "назад" на андроиде

    //Событие котороне вызывается при нажатии кнопки
    private static event Action GoBack;

    private void Start()
    {
        //Обработчик события по умолчанию
        GoBack += MoveApplicationToBack;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !PanelAnimation.IsAnyAnimationRunning) //IsAnyAnimationRunning нужен чтобы
        {                                                                            //Кнопка срабатывала только после анимации
            if (GoBack == null)
                throw new ArgumentNullException("GoBack", "Event GoBack has no handlers");
            
            GoBack();
            RemoveEventHandler();
        }
    }

    public void SetEventHandler(PanelAnimation animation)
    {
        //У события всегда должен быть 1 обработчик
        if (GoBack != null)
            GoBack = null;

        GoBack += animation.ChangeAnimationState;
    }

    private void RemoveEventHandler()
    {
        GoBack = null;
        //Обработчик по умолчанию
        GoBack += MoveApplicationToBack;
    }

    private void MoveApplicationToBack()
    {
        //Сворачивает приложение
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
    }
}
