using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region STATIC
    public static UIManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

}
