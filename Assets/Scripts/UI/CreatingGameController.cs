using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatingGameController : MonoBehaviour
{
    [SerializeField] private Slider _countPlayer;
    
    private Animator _animator;
    private GameSession _session;
    private int _mode = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _session = FindObjectOfType<GameSession>();
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
                _session.SetQuantityOfPlayers((int)_countPlayer.value);
                SceneManager.LoadScene("Game");
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
}



