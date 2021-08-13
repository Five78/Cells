using System;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{
    private static event Action GoBack;

    private void Start()
    {
        GoBack += MoveApplicationToBack;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !PanelAnimation.IsAnyAnimationRunning)
        {
            if (GoBack == null)
                throw new ArgumentNullException("GoBack", "Event GoBack has no handlers");
            
            GoBack();
            RemoveEventHandler();
        }
    }

    public void SetEventHandler(PanelAnimation animation)
    {
        if (GoBack != null)
            GoBack = null;

        GoBack += animation.ChangeAnimationState;
    }

    private void RemoveEventHandler()
    {
        GoBack = null;
        GoBack += MoveApplicationToBack;
    }

    private void MoveApplicationToBack()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
    }
}
