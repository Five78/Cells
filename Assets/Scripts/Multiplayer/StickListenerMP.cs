using UnityEngine;
using Firebase.Database;

public class StickListenerMP : StickListener
{
    private static DatabaseReference _lobby;
    public StickCoordinates coordinates;

    private void Awake()
    {
        _lobby = FirebaseDatabase.DefaultInstance.GetReference("onGoingLobbies").Child(GameSession.Instance.UserLobby.Id);
    }

    public override void OnClick()
    {
        base.OnClick();
        SendMoveCoorinates();
    }

    private void SendMoveCoorinates()
    {
        string coordinatesJson = JsonUtility.ToJson(coordinates);
        _lobby.Child("stickCoordinates").SetRawJsonValueAsync(coordinatesJson);
    }
}
