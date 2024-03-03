using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSFXEvent : MonoBehaviour
{
    public void PlayCountdownSFX()
    {
        AudioManager.Instance.PlaySFX("Countdown_SFX", 0.3f);
    }

    public void PlayFireSFX()
    {
        AudioManager.Instance.PlaySFX("Blast_Off_SFX", 0.3f);
    }
}
