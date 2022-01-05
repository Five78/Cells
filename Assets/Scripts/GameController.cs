using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerIcon; 
    [SerializeField] private GameObject[] _playerSelected;
    [SerializeField] private Text _timer;
    [SerializeField] private GameObject _endGame;
    [Space]
    [SerializeField] private CellComponent[] _cells;
    [SerializeField] private StickListener[] _verticalSticks;
    [SerializeField] private StickListener[] _horizontalSticks;

    private GameSession _session;
    private int _playersNumber;
    private int _size;
    private bool _playerMovesAgain;
    private float _time;
    private int _countPoint;
    private List<int> _banList = new List<int>();

    private void Start()
    {
        _session = GameSession.Instance;
        _playersNumber = 1;

        if (_session == null)
        {
            var sessin = Resources.Load<GameObject>("GameSession");
            Object.Instantiate(sessin);
            _session = GameSession.Instance;
        }
        
        for (int i = 0; i < _session.QuantityOfPlayers; i++)
        {
            _playerIcon[i].SetActive(true);
        }

        _countPoint = _cells.Length;

        _playerSelected[0].SetActive(true);
        _timer.text = "00";
        _time = _session.Timer;

        _size = (int)Math.Sqrt(_cells.Length);

        StartCoroutine(StartTimer());
    }

    public Color WhoseMoveNow()
    {
        switch (_playersNumber)
        {
            case 1:
                return _session.Player1;
            case 2:
                return _session.Player2;
            case 3:
                return _session.Player3;
            case 4:
                return _session.Player4;
            default:
                return _session.Player1;
        }
    }

    public void MoveIsMade()
    {
        _playerMovesAgain = false;
        
        StopAllCoroutines();
        StartCoroutine(StartTimer()); //сбрасываем таймер при ходе. можно изменить через _temi = _delay

        CheckingTheCell();

        if (!_playerMovesAgain)
        {
            _playerMovesAgain = false;

            _playerSelected[_playersNumber - 1].SetActive(false);
            ChangeMove();
            _playerSelected[_playersNumber - 1].SetActive(true);
        }
    }


    private float _delay;
    private IEnumerator StartTimer()
    {
        _delay = _time;
        _time = _session.Timer;
        while (_delay >= 0)
        {
            _timer.text = $"{_delay}";
            _delay--;
            yield return new WaitForSeconds(1f);
        }
        _banList.Add(_playersNumber);
        if (_banList.Count == _session.QuantityOfPlayers - 1)
        {
            EndGameWhitBan();
            StopAllCoroutines();
        }
        else            
            MoveIsMade();
    }

    public void ChangeMove()
    {
        if (_playersNumber < _session.QuantityOfPlayers)
            _playersNumber++;
        else
            _playersNumber = 1;
        if (_banList.Contains(_playersNumber))
            ChangeMove();
    }

    public void CheckingTheCell()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            int a = i + _size;
            int b = i + (i / _size);
            int c = b + 1;
            if (_horizontalSticks[i].Standing && _horizontalSticks[a].Standing && _verticalSticks[b].Standing && _verticalSticks[c].Standing)
                ChangingTheCell(i);
        }
    }

    private void ChangingTheCell(int i)
    {
        if (!_cells[i].ChangedTheColor)
        {
            _playerMovesAgain = true;
            _cells[i].ChangeColor(WhoseMoveNow());
            _session.SetPoint(_playersNumber - 1);

            _countPoint--;
            if (_countPoint == 0)
                EndGame();
        }
    }

    public void OnPause()
    {
        _time = _delay;
        StopAllCoroutines();
    }

    public void OnResume()
    {
        StartCoroutine(StartTimer());
    }

    public void BackToMenu()
    {
        _session.ClearPoints();
        _session.DestroySession();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnSettingWindow()
    {
        string path = "SettingWindow";
        var window = Resources.Load<GameObject>(path);
        var canvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();

        Object.Instantiate(window, canvas.transform);
    }

    private void EndGame()
    {
        StopAllCoroutines();
        _endGame.SetActive(true);
    }

    private void EndGameWhitBan()
    {
        StopAllCoroutines();
        if (_playersNumber < _session.QuantityOfPlayers)
            _playersNumber++;
        else
            _playersNumber = 1;
        
        foreach(var item in _cells)
        {
            item.ChangeColor(WhoseMoveNow());
            _session.SetPoint(_playersNumber - 1);
        }

        _endGame.SetActive(true);
    }
}
