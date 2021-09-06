using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameControllerAI : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerIcon;
    [SerializeField] private GameObject[] _playerSelected;
    [SerializeField] private GameObject _block;
    [SerializeField] private Text _timer;
    [SerializeField] private GameObject _endGame;
    [Space]
    [SerializeField] private CellComponent[] _cells;
    [SerializeField] private StickListener[] _verticalSticks;
    [SerializeField] private StickListener[] _horizontalSticks;
    [Space(20f)]
    [SerializeField] private float _delayAIMax = 3f;

    private GameSession _session;
    private int _playersNumber;
    private int _countPoint;
    private int _size;
    private float _time;
    private float _AILevel;
    private bool _playerMovesAgain;
    private bool _banUser = false;

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

        for (int i = 0; i < _session.QuantityOfAI; i++)
        {
            _playerIcon[i].SetActive(true);
        }

        _playerSelected[0].SetActive(true);
        _block.SetActive(false);
        _timer.text = "00";


        _countPoint = _cells.Length;
        _size = (int)Math.Sqrt(_cells.Length);
        _time = _session.Timer;
        _AILevel = _session.AILevel;

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
        _block.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(StartTimer()); //сбрасываем таймер при ходе. можно изменить через _temi = _delay

        CheckingTheCell();

        if (!_playerMovesAgain)
        {
            _playerMovesAgain = false;

            _playerSelected[_playersNumber - 1].SetActive(false);
            ChangeMove();
            _playerSelected[_playersNumber - 1].SetActive(true);

            if (_playersNumber != 1)
                AIMove();
        }
        if (_playerMovesAgain && _playersNumber != 1)
            AIMove();
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
        _banUser = true;
        EndGameWhitBan();
    }

    public void ChangeMove()
    {
        if (_playersNumber < _session.QuantityOfAI)
            _playersNumber++;
        else
            _playersNumber = 1;
        if (_banUser && _playersNumber == 1)
            _playersNumber++;
    }

    public void CheckingTheCell()
    {
        int a, b, c;
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].ChangedTheColor)
                continue;
            a = i + _size;
            b = i + (i / _size);
            c = b + 1;
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

    private void AIMove()
    {
        _block.SetActive(true);
        StartCoroutine(AI());
    }

    private IEnumerator AI()
    {
        yield return new WaitForSeconds(_delayAIMax);

        float chance = Random.Range(0f, 1f);

        bool flag = true;

        if (_AILevel >= 0.25f)
            if (chance <= _AILevel)
            {
                for (int i = 0; i < _cells.Length; i++)  //ищем, где стоят три палочки на плитке
                {
                    if (_cells[i].ChangedTheColor)
                        continue;

                    int a = i + _size;
                    int b = i + (i / _size);
                    int c = b + 1;
                    if (_horizontalSticks[i].Standing && _horizontalSticks[a].Standing && _verticalSticks[b].Standing)
                    {
                        _verticalSticks[c].OnClick();
                        flag = false;
                        break;
                    }
                    else if (_horizontalSticks[i].Standing && _horizontalSticks[a].Standing && _verticalSticks[c].Standing)
                    {
                        _verticalSticks[b].OnClick();
                        flag = false;
                        break;
                    }
                    else if (_horizontalSticks[i].Standing && _verticalSticks[b].Standing && _verticalSticks[c].Standing)
                    {
                        _horizontalSticks[a].OnClick();
                        flag = false;
                        break;
                    }
                    else if (_horizontalSticks[a].Standing && _verticalSticks[b].Standing && _verticalSticks[c].Standing)
                    {
                        _horizontalSticks[i].OnClick();
                        flag = false;
                        break;
                    }             
                }

                if (flag && _AILevel >= 0.75f)
                {
                    List<int> randomCells = new List<int>();
                    while (randomCells.Count != _cells.Length)
                    {
                        int rand = Random.Range(0, _cells.Length);
                        if (!randomCells.Contains(rand)) randomCells.Add(rand);
                    }

                    List<bool> temp_H = new List<bool>();
                    foreach (var item in _horizontalSticks)
                        temp_H.Add(item.Standing);

                    List<bool> temp_V = new List<bool>();
                    foreach (var item in _verticalSticks)
                        temp_V.Add(item.Standing);

                    List<StickListener> availableStick = new List<StickListener>();

                    foreach (int i in randomCells) //ищем, где нет палочек на плитке
                    {
                        if (_cells[i].ChangedTheColor)
                            continue;
                        int a = i + _size;
                        int b = i + (i / _size);
                        int c = b + 1;

                        if (!temp_H[i] && !temp_H[a] && !temp_V[b] && !temp_V[c])
                        {
                            temp_H[i] = true;
                            if (CheckingTheCopy(_cells, temp_H, temp_V))
                                availableStick.Add(_horizontalSticks[i]);
                            temp_H[i] = false;
                            temp_H[a] = true;
                            if (CheckingTheCopy(_cells, temp_H, temp_V))
                                availableStick.Add(_horizontalSticks[a]);
                            temp_H[a] = false;
                            temp_V[b] = true;
                            if (CheckingTheCopy(_cells, temp_H, temp_V))
                                availableStick.Add(_verticalSticks[b]);
                            temp_V[b] = false;
                            temp_V[c] = true;
                            if (CheckingTheCopy(_cells, temp_H, temp_V))
                                availableStick.Add(_verticalSticks[c]);
                            temp_V[c] = false;
                        }
                    }
                    
                    if (availableStick.Count != 0)
                    {
                        var randIndex = Random.Range(0, availableStick.Count);
                        availableStick[randIndex].OnClick();
                        flag = false;
                    }

                    if (flag)
                    {
                        foreach (int i in randomCells)
                        {
                            if (_cells[i].ChangedTheColor)
                                continue;
                            int a = i + _size;
                            int b = i + (i / _size);
                            int c = b + 1;

                            if (temp_H[i] && !temp_H[a] && !temp_V[b] && !temp_V[c])
                            {
                                temp_H[a] = true;
                                if(CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[a].OnClick();
                                    flag = false;
                                    break;
                                }

                                temp_H[a] = false;
                                temp_V[b] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[b].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[b] = false;
                                temp_V[c] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[c].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[c] = false;

                            }
                            if (!temp_H[i] && temp_H[a] && !temp_V[b] && !temp_V[c])
                            {
                                temp_H[i] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[i].OnClick();
                                    flag = false;
                                    break;
                                }

                                temp_H[i] = false;
                                temp_V[b] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[b].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[b] = false;
                                temp_V[c] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[c].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[c] = false;
                            }
                            if (!temp_H[i] && !temp_H[a] && temp_V[b] && !temp_V[c])
                            {
                                temp_H[i] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[i].OnClick();
                                    flag = false;
                                    break;
                                }

                                temp_H[i] = false;
                                temp_H[a] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[a].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_H[a] = false;
                                temp_V[c] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[c].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[c] = false;
                            }
                            if (!temp_H[i] && !temp_H[a] && !temp_V[b] && temp_V[c])
                            {
                                temp_H[i] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[i].OnClick();
                                    flag = false;
                                    break;
                                }

                                temp_H[i] = false;
                                temp_H[a] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _horizontalSticks[a].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_H[a] = false;
                                temp_V[b] = true;
                                if (CheckingTheCopy(_cells, temp_H, temp_V))
                                {
                                    _verticalSticks[b].OnClick();
                                    flag = false;
                                    break;
                                }
                                temp_V[b] = false;
                            }
                        }
                    }
                }
            }

        while (flag)
        {
            bool horizont = Random.Range(0, 2) == 1 ? true : false;
            if (horizont)
            {
                int i = Random.Range(0, _horizontalSticks.Length);
                if (!_horizontalSticks[i].Standing)
                {
                    _horizontalSticks[i].OnClick();
                    break;
                }
            }
            else
            {
                int i = Random.Range(0, _verticalSticks.Length);
                if (!_verticalSticks[i].Standing)
                {
                    _verticalSticks[i].OnClick();
                    break;
                }
            }
            yield return null;
        }
    }

    private bool CheckingTheCopy(CellComponent[] cells, List<bool> temp_H, List<bool> temp_V)
    {
        for (int i = 0; i < cells.Length; i++)  //ищем, где стоят три палочки на плитке
        {
            if (cells[i].ChangedTheColor)
            continue;

            int a = i + _size;
            int b = i + (i / _size);
            int c = b + 1;
            if (temp_H[i] && temp_H[a] && temp_V[b])
            {
                return false;
            }
            else if (temp_H[i] && temp_H[a] && temp_V[c])
            {
                return false;
            }
            else if (temp_H[i] && temp_V[b] && temp_V[c])
            {
                return false;
            }
            else if (temp_H[a] && temp_V[b] && temp_V[c])
            {
                return false;
            }
        }
        return true;
    }


    private void EndGameWhitBan()
    {
        _delayAIMax = 0.1f;
        ChangeMove();
        AIMove();
    }

    #region кнопки
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
    #endregion
}
