using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameWindow : MonoBehaviour
{
    [SerializeField] private Text[] _players;

    private void OnEnable()
    {
        var session = FindObjectOfType<GameSession>();
        var count = session.QuantityOfPlayers;
        int max = 0;
        int indexMax = 0;

        for (int i = 0; i < count; i++)
        {
            _players[i].gameObject.SetActive(true);
            _players[i].text = $"pl{i + 1} : {session.PlayersPoints[i]}";
            if (session.PlayersPoints[i] > max)
            {
                max = session.PlayersPoints[i];
                indexMax = i;
            }                
        }

        _players[indexMax].color = session.Player1;
    }

    public void OnExit()
    {
        FindObjectOfType<GameSession>().ClearPoints();
        SceneManager.LoadScene("MainMenu");
    }
}
