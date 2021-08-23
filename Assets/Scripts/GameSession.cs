using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int _Count;
    
    [SerializeField] private Color _player1;
    [SerializeField] private Color _player2;
    [SerializeField] private Color _player3;
    [SerializeField] private Color _player4;

    public Color Player1 => _player1;
    public Color Player2 => _player2;
    public Color Player3 => _player3;
    public Color Player4 => _player4;

    public int QuantityOfPlayers { get; private set; } = 4;
    //private int _move;

    private void Awake()
    {
        QuantityOfPlayers = 4;
        //DontDestroyOnLoad(this);
    }
}
