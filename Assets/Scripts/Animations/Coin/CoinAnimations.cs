using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimations : MonoBehaviour
{
    const string CATCH_ANIM = "catch";

    public void PlayCatch()
    {
        Debug.Log("play catch");
        GetComponent<Animator>().Play(CATCH_ANIM);
    }
}
