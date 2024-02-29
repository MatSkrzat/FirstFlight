using Assets.Scripts.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPanelAnimations : MonoBehaviour
{
    const string PANEL_OPEN = "hidingPanelOpen";
    const string PANEL_CLOSE = "hidingPanelClose";
    const string CLOSED = "closed";

    private AnimationController animationController;

    private void Awake()
    {
        animationController = new AnimationController(GetComponent<Animator>());
    }

    public void PlayOpen()
    {
        animationController.Play(PANEL_OPEN);
    }

    public void PlayClose()
    {
        animationController.Play(PANEL_CLOSE);
    }
}
