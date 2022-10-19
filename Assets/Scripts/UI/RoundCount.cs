using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCount : MonoBehaviour
{
    private Text roundText;

    public void UpdateRoundCounter(int round)
    {
        if (roundText == null)
        {
            roundText = GetComponent<Text>();

            roundText.text = ("Wave: " + round);
        }
        else
        {
            roundText.text = ("Wave: " + round);
        }
    }
}
