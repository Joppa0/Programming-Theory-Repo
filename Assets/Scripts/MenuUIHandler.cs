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
    public string playerName;

    public InputField inputField;

    public Text highScore;

    // Start is called before the first frame update
    void Start()
    {
        WinnerList.instance.LoadWinnerData();
        highScore.text = "Best Score: " + WinnerList.instance.bestScore;
    }

    public void SaveName()
    {
        playerName = inputField.text;
        WinnerList.instance.playerName = playerName;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
}
