﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class CreatingGameController : MonoBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Slider _countPlayer;
    [SerializeField] private Slider _countAI;
    [SerializeField] private Slider _AILevel;
    [SerializeField] private GameObject[] _playerList;

    private Animator _animator;
    private GameSession _session;
    private int _mode = 0;
    private int _sizeBoard = 0;
    private int _sliderValue;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _session = FindObjectOfType<GameSession>();
        _sliderValue = (int)_countPlayer.value;
    }

    public void ActiveMode(int mode)
    {
        _mode = mode;
    }

    public void ActiveSizeBoard(int size)
    {
        _sizeBoard = size;
    }

    public void StartGame()
    {
        switch (_mode)
        {
            case 0:
                _session.QuantityOfAI = (int)_countAI.value+1;
                _session.AILevel = _AILevel.value;
                _session.AIMode = true;
                switch (_sizeBoard)
                {
                    case 0:
                        SceneManager.LoadScene("Game3AI");
                        break;
                    case 1:
                        SceneManager.LoadScene("Game5AI");
                        break;
                    case 2:
                        break;
                    default:
                        SceneManager.LoadScene("Game5AI");
                        break;
                }                
                break;
            case 1:
                break;
            case 2:
                if ((int)_countPlayer.value > 1)
                {
                    _session.QuantityOfPlayers = (int)_countPlayer.value;
                    switch (_sizeBoard)
                    {
                        case 0:
                            SceneManager.LoadScene("Game3");
                            break;
                        case 1:
                            SceneManager.LoadScene("Game5");
                            break;
                        case 2:
                            break;
                        default:
                            SceneManager.LoadScene("Game5");
                            break;
                    }
                    
                }
                break;
        }
    }

    public void Close()
    {
        _animator.SetTrigger("close");
    }

    public void OnCloseAnimatorComplete()
    {
        Destroy(gameObject);
    }

    public void OnSettingWindow()
    {
        string path = "SettingWindow";
        var window = Resources.Load<GameObject>(path);
        var canvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();

        Object.Instantiate(window, canvas.transform);
    }

    public void OnCreateLobby()
    {
        string[] user = new string[] { _session.GetName()[0] };
        LobbyDB data = new LobbyDB(_lobbyName.text, user);

        LobbyWaiting.Instantiate(data);
    }

    public void Update()
    {
        if(_sliderValue != (int)_countPlayer.value)
        {
            _sliderValue = (int)_countPlayer.value;
            foreach (var item in _playerList)
                item.SetActive(false);
            for (int i = 0; i < _sliderValue; i++)
            {
                _playerList[i].SetActive(true);
            }
        }
    }
}



