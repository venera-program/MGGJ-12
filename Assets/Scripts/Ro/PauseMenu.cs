using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        PlayerControllerScript.instance.DisablePlayerControls();
        Graze.instance.StopGrazeCount();
        Time.timeScale = 0f;
    }

    public void OnDisable()
    {
        Time.timeScale = 1f;
        Graze.instance.StartGrazeCount();
        PlayerControllerScript.instance.EnablePlayerControls();
    }

}