using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainManager : MonoBehaviour
{
    public Text ScoreText;
    public Text PlayerName;
    public Text BestScore;
    public GameObject GameOverText;

    private bool m_GameOver = false;
    private int m_Points;
    void Start()
    {
        PlayerName.text = "Player Name: " + WinnerList.instance.playerName;
        SetBestPlayer();
    }
    void Update()
    {
        if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        WinnerList.instance.score = m_Points;
    }

    public void CheckBestPlayer()
    {
        if (WinnerList.instance.score >= WinnerList.instance.bestScore)
        {
            WinnerList.instance.bestPlayer = WinnerList.instance.playerName;
            WinnerList.instance.bestScore = WinnerList.instance.score;
        }
        WinnerList.instance.SaveWinnerData(WinnerList.instance.bestPlayer, WinnerList.instance.bestScore);
    }

    public void SetBestPlayer()
    {
        if (WinnerList.instance.bestPlayer == null && WinnerList.instance.bestScore == 0)
        {
            BestScore.text = " ";
        }
        else
        {
            BestScore.text = "Best Score: " + WinnerList.instance.bestPlayer + ": " + WinnerList.instance.bestScore;
        }
    }
}
