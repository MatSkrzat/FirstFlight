using Assets.Scripts.Animations;
using UnityEngine;

public class ShieldAnimations : MonoBehaviour
{
    const string LOAD_ANIM = "load";
    const string HIT_ANIM = "hit";
    const string UNLOAD_ANIM = "unload";

    private AnimationController animationController;

    private void Awake()
    {
        animationController = new AnimationController(GetComponent<Animator>());
    }

    public void PlayLoad()
    {
        animationController.Play(LOAD_ANIM);
    }

    public void PlayHit()
    {
        animationController.Play(HIT_ANIM);
    }

    public void PlayUnload()
    {
        animationController.Play(UNLOAD_ANIM);
    }
}
