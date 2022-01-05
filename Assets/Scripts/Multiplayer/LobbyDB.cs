using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class LobbyDB
{
    public static DatabaseReference AvailableLobbies => FirebaseDatabase.DefaultInstance.RootReference.Child("lobbies");
    public static DatabaseReference OnGoingLobbies => FirebaseDatabase.DefaultInstance.RootReference.Child("onGoingLobbies");

    private readonly Data _lobbyData;

    public string Id => _lobbyData.id;
    public bool OwnedByUser => _lobbyData.ownedByUser;
    public string Name => _lobbyData.name;
    public string[] Players => _lobbyData.players;

    public LobbyDB(string name, string[] players)
    {
        _lobbyData = new Data(name, players);
        _lobbyData.ownedByUser = true;
        GameSession.Instance.SetLobby(this);

        string lobbyJson = JsonUtility.ToJson(_lobbyData);
        AvailableLobbies.Child(_lobbyData.id).SetRawJsonValueAsync(lobbyJson);
    }

    public LobbyDB(DataSnapshot snapshot)
    {
        string lobbyDBJson = snapshot.GetRawJsonValue();
        _lobbyData = JsonUtility.FromJson<Data>(lobbyDBJson);
        _lobbyData.id = snapshot.Key;
    }

    public void Join()
    {
        string userName = GameSession.Instance.GetName()[0];
        string userIndex = _lobbyData.players.Length.ToString();

        AvailableLobbies.Child(_lobbyData.id)
            .Child("players")
            .Child(userIndex)
            .SetValueAsync(userName);

        LobbyWaiting.Instantiate(this);
    }

    public void Leave()
    {
        string userIndex = (_lobbyData.players.Length - 1).ToString();

        AvailableLobbies.Child(_lobbyData.id)
            .Child("players")
            .Child(userIndex)
            .SetValueAsync(null);
    }

    public void StartLobby()
    {
        OnGoingLobbies.Child(_lobbyData.id).Child("stickCoordinates").SetValueAsync("empty");

        RemoveLobbyFromDB();
    }

    public void IfDoesntExist(Action action)
    {
        AvailableLobbies.Child(_lobbyData.id).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result.Exists)
                        action();
                }
            });
    }

    public void RemoveLobbyFromDB()
    {
        AvailableLobbies.Child(_lobbyData.id).SetValueAsync(null);
    }

    public void NullifyLocalPlayersData()
    {
        _lobbyData.players = default;
    }

    public void AddPlayer(DataSnapshot snapshot)
    {
        string player = snapshot.Value.ToString();

        if (_lobbyData.players == default)
        {
            _lobbyData.players = new string[] { player };
            return;
        }

        string[] newArray = new string[_lobbyData.players.Length + 1];

        for (int i = 0; i < _lobbyData.players.Length; i++)
            newArray[i] = _lobbyData.players[i];
        newArray[newArray.Length - 1] = player;

        _lobbyData.players = newArray;
    }

    [Serializable]
    private class Data
    {        
        [NonSerialized] public string id;
        [NonSerialized] public bool ownedByUser;

        public string name;
        public string[] players;

        public Data(string name, string[] players)
        {
            id = GenerateId();

            this.name = name;
            this.players = players;
        }

        private static string GenerateId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
