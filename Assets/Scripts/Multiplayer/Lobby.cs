using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Text _lobbyName;

    private LobbyDB _lobbyDB;

    private void Start()
    {
        _lobbyName.text = _lobbyDB.Name;

        LobbyDB.AvailableLobbies.ChildRemoved += HandleLobbyRemoval;
    }

    public void Join()
    {
        _lobbyDB.Join();
    }

    public static void Instantiate(DataSnapshot snapshot, GameObject lobbyUI, Transform parent)
    {
        Instantiate(lobbyUI, parent).GetComponent<Lobby>()
            ._lobbyDB = new LobbyDB(snapshot);
    }

    private void HandleLobbyRemoval(object sender, ChildChangedEventArgs args)
    {
        _lobbyDB.IfDoesntExist(() => Destroy(gameObject));
    }
}