﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private float _timer = 30f;

    [SerializeField] private Color _player1;
    [SerializeField] private Color _player2;
    [SerializeField] private Color _player3;
    [SerializeField] private Color _player4;
    [Space]
    [SerializeField] private string _nameUser = "User";
    [SerializeField] private string _nameAI1 = "Алиса";
    [SerializeField] private string _nameAI2 = "Боб";
    [SerializeField] private string _nameAI3 = "Ева";

    public static GameSession Instance { get; private set; }

    public LobbyDB UserLobby { get; private set; }

    public float Timer => _timer;

    public Color Player1 => _player1;
    public Color Player2 => _player2;
    public Color Player3 => _player3;
    public Color Player4 => _player4;

    public int[] PlayersPoints { get; private set; } = new int[4];

    public int QuantityOfPlayers { get; set; } = 2;
    public int QuantityOfAI { get; set; } = 4;
    public float AILevel { get; set; } = 1f;
    public bool AIMode { get; set; } = false;

    private void Awake()
    {
        ClearPoints();
        Instance = this;

        DontDestroyOnLoad(this);
    }

    public void SetLobby(LobbyDB lobby)
    {
        UserLobby = lobby;

        QuantityOfPlayers = lobby.Players.Length;
        _nameAI1 = QuantityOfPlayers == 2 ? lobby.Players[1] : _nameAI1;
        _nameAI2 = QuantityOfPlayers == 3 ? lobby.Players[2] : _nameAI2;
        _nameAI3 = QuantityOfPlayers == 4 ? lobby.Players[3] : _nameAI3;
    }

    public void SetPoint(int index)
    {
        PlayersPoints[index] += 1;
    }

    public string[] GetName()
    {
        return new string[4] { _nameUser , _nameAI1 , _nameAI2, _nameAI3 };        
    }

    public void ClearPoints()
    {
        for (int i = 0; i < PlayersPoints.Length; i++)
        {
            PlayersPoints[i] = 0;
        }
    }

    public void DestroySession()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
