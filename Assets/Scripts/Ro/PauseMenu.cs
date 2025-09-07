using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0f;
        PlayerControllerScript.instance.DisablePlayerControls();
    }

    public void OnDisable()
    {
        Time.timeScale = 1f;
        PlayerControllerScript.instance.EnablePlayerControls();
    }

}