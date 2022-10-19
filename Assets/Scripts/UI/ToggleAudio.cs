using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool toggleMusic, toggleEffects;

    [SerializeField] private Image panelImage;

    Color backgroundColor = new Color(18f / 255f, 36f / 255f, 63f / 255f, 255);

    public void Toggle()
    {
        if (panelImage.color == Color.white)
        {
            panelImage.color = backgroundColor;
        }

        else if (panelImage.color == backgroundColor)
        {
            panelImage.color = Color.white;
        }

        if (toggleEffects)
        {
            SoundManager.instance.ToggleEffects();
        }
        if (toggleMusic)
        {
            SoundManager.instance.ToggleMusic();
        }
    }
}
