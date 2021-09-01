using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private float _timer;

    [SerializeField] private Color _player1;
    [SerializeField] private Color _player2;
    [SerializeField] private Color _player3;
    [SerializeField] private Color _player4;
    public static GameSession Instance { get; private set; }
    public float Timer => _timer;

    public Color Player1 => _player1;
    public Color Player2 => _player2;
    public Color Player3 => _player3;
    public Color Player4 => _player4;

    public int[] PlayersPoints { get; private set; } = new int[4];

    public int QuantityOfPlayers { get; private set; } = 2;

    private void Awake()
    {
        ClearPoints();
        Instance = this;

        DontDestroyOnLoad(this);
    }

    public void SetPoint(int index)
    {
        PlayersPoints[index] += 1;
    }

    public void SetQuantityOfPlayers(int count)
    {
        QuantityOfPlayers = count;
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
