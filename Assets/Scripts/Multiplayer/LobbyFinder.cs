using Firebase;
using Firebase.Database;
using UnityEngine;

public class LobbyFinder : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _lobbyUI;

    private void Start()
    {
        LobbyDB.AvailableLobbies.ChildAdded += 
            (object sender, ChildChangedEventArgs args) => { Lobby.Instantiate(args.Snapshot, _lobbyUI, _parent); };
    }
}
