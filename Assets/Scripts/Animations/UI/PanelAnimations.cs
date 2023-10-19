using Assets.Scripts.Animations;
using UnityEngine;

public class PanelAnimations : MonoBehaviour
{
    const string OPEN_ANIM = "open";
    const string EXIT_ANIM = "exit";
    const string IDLE_ANIM = "idle";
    const string REFRESH_ANIM = "refresh";

    private AnimationController animationController;

    private void Start()
    {
        animationController = new AnimationController(GetComponent<Animator>());
    }

    public void PlayOpen()
    {
        animationController.Play(OPEN_ANIM);
    }

    public void PlayClose()
    {
        animationController.Play(EXIT_ANIM);
    }

    public void PlayRefresh()
    {
        animationController.Play(REFRESH_ANIM);
    }
}
