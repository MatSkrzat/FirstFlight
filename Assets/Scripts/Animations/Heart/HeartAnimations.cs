using Assets.Scripts.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimations : MonoBehaviour
{
    const string CHANGE_ANIM = "change";

    public void PlayChange()
    {
        GetComponent<Animator>().Play(CHANGE_ANIM);
    }
}
