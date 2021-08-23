using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameSession _session;
    private int _count;
    //private bool _canMove = true;



    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _count = 1;
    }

    public Color WhoseMoveNow()
    {
        switch (_count)
        {
            case 1:
                SwapMove();
                return _session.Player1;
            case 2:
                SwapMove();
                return _session.Player2;
            case 3:
                SwapMove();
                return _session.Player3;
            case 4:
                SwapMove();
                return _session.Player4;
            default:
                return _session.Player4; //защита от пустого хода
        }        
    }

    private void SwapMove()
    {
        if (_count < _session.QuantityOfPlayers)
            _count++;
        else
            _count = 1;
    }
}
