using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    public void EndZapAnim()
    {
        GameManager.Instance.EndZapAnim();
    }

    public void EndLostAnim()
    {
        GameManager.Instance.EndLostAnim();
    }

    public void EndYayAnim()
    {
        GameManager.Instance.EndYayAnim();
    }
}
