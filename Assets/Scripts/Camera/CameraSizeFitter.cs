using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeFitter : MonoBehaviour
{
    #region fields
    /// <summary>
    /// Default width for portrait
    /// </summary>
    [SerializeField]
    private float defaultWidth = 1080;
    /// <summary>
    /// Default height for portrait
    /// </summary>
    [SerializeField]
    private float defaultHeight = 1920;
    /// <summary>
    /// Camera size
    /// </summary>
    [SerializeField]
    private float defaultCameraSize = 5;
    [SerializeField]
    private bool isPortrait = true;

    private float defaultAspectRatio;
    private float screenAspectRatio;
    #endregion

    #region init
    private void Awake()
    {
        if (isPortrait)
        {
            defaultAspectRatio = defaultHeight / defaultWidth;
            screenAspectRatio = (float)Screen.height / Screen.width;
        }
        else
        {
            defaultAspectRatio = defaultHeight / defaultWidth;
            screenAspectRatio = (float)Screen.width / Screen.height;
        }
        gameObject.GetComponent<Camera>().orthographicSize = defaultCameraSize * (screenAspectRatio / defaultAspectRatio);
    }
    #endregion
}
