using UnityEngine;

public class BonusAnimations : MonoBehaviour
{
    const string CATCH_ANIM = "catch";

    public void PlayCatch()
    {
        GetComponent<Animator>().Play(CATCH_ANIM);
    }
}
