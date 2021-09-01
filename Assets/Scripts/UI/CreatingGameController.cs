using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class CreatingGameController : MonoBehaviour
{
    [SerializeField] private Slider _countPlayer;
    [SerializeField] private GameObject[] _playerList;

    private Animator _animator;
    private GameSession _session;
    private int _mode = 0;
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

    public void StartGame()
    {
        switch (_mode)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                if ((int)_countPlayer.value > 1)
                {
                    _session.SetQuantityOfPlayers((int)_countPlayer.value);
                    SceneManager.LoadScene("Game");
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



