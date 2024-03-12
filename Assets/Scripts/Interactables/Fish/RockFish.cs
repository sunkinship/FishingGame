using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFish : BaseFish
{
    public bool IsShattered {  get; private set; }  

    public void Shatter()
    {
        IsShattered = true;
        GameManager.Instance.SetHookedRockFish(false);
        anim.SetTrigger("Shatter");
    }

    public override void Hooked(Transform catchPoint)
    {
        base.Hooked(catchPoint);
        GameManager.Instance.SetHookedRockFish(true);
    }

    public override void Escape()
    {
        base.Escape();
        GameManager.Instance.SetHookedRockFish(false);
    }
}
