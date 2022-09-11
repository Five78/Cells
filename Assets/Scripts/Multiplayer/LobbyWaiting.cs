using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class LobbyWaiting : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private GameObject _startButtonBlock;
    [SerializeField] private GameObject[] _players;

    private LobbyDB _lobbyDB;

    private Animator _animator;
    private int _sizeBoard = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public static void Instantiate(LobbyDB lobby)
    {
        string path = "LobbyWaiting";
        var window = Resources.Load<GameObject>(path);
        var canvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();

        LobbyWaiting lobbyWaiting = Instantiate(window, canvas.transform).GetComponent<LobbyWaiting>();
        lobbyWaiting.SetLobbyData(lobby);
    }

    private void SetLobbyData(LobbyDB lobby)
    {
        _lobbyDB = lobby;

        _lobbyDB.NullifyLocalPlayersData();
        _name.text = _lobbyDB.Name;

        LobbyDB.AvailableLobbies.Child(_lobbyDB.Id).Child("players").ChildAdded += AddPlayerUI;

        if (!_lobbyDB.OwnedByUser)
        {
            FirebaseDatabase.DefaultInstance.GetReference("onGoingLobbies")
                .ChildAdded += (object sender, ChildChangedEventArgs args) =>
                {
                    if (args.Snapshot.Key == _lobbyDB.Id)
                        StartLobby();
                };
        }
    }

    public void StartLobby()
    {
        _lobbyDB.MoveLobbyToOnGoingBranch();
        GameSession.Instance.SetLobby(_lobbyDB);

        switch (_sizeBoard)
        {
            case 0:
                SceneManager.LoadScene("Game3MP");
                break;

            case 1:
                SceneManager.LoadScene("Game5MP");
                break;
        }
    }


    private void AddPlayerUI(object sender, ChildChangedEventArgs args)
    {
        _lobbyDB.AddPlayer(args.Snapshot);
        TryEnableStartButton();

        string playerName = args.Snapshot.Value.ToString();
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i].activeSelf == false)
            {
                _players[i].GetComponentInChildren<Text>().text = playerName;
                _players[i].SetActive(true);
                return;
            }
        }
    }

    private void TryEnableStartButton()
    {
        if (!_startButtonBlock.activeSelf)
            return;

        if (_lobbyDB.OwnedByUser && _lobbyDB.Players.Length >= 2)
            _startButtonBlock.SetActive(false);
    }

    #region кнопки

    public void ActiveSizeBoard(int size)
    {
        _sizeBoard = size;
    }

    public void OnSettingWindow()
    {
        string path = "SettingWindow";
        var window = Resources.Load<GameObject>(path);
        var canvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();

        Object.Instantiate(window, canvas.transform);
    }

    public void OnCloseAnimatorComplete()
    {
        if (_lobbyDB.OwnedByUser)
            _lobbyDB.RemoveLobbyFromBranch(LobbyDB.LobbyBranch.Awaiting);
        else
            _lobbyDB.Leave();
            
        Destroy(gameObject);
    }

    public void Close()
    {
        _animator.SetTrigger("close");
    }

    #endregion
}
