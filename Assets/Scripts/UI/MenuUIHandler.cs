using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    private string playerName;

    public InputField inputField;

    public Text highScore;

    // Start is called before the first frame update
    void Start()
    {
        //Loads the winner and sets the highscore to that value
        WinnerList.instance.LoadWinnerData();
        highScore.text = "Best Score: " + WinnerList.instance.bestScore;
    }

    public void SaveName()
    {
        //Saves name to be used in the game
        playerName = inputField.text;
        WinnerList.instance.playerName = playerName;
    }

    public void StartNew()
    {
        //Loads the next scene when the start button is pressed
        SceneManager.LoadScene(1);
    }
}
