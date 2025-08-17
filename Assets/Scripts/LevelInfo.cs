using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    [Header("Stage Assets")]
    public Sprite newBackgroundTexture;
    public Image backgroundImageObj;

    // [Header("Spawner Info")]

    // [Header("Cutscene Info")]

    public void Initialize()
    {
        backgroundImageObj.sprite = newBackgroundTexture;
    }
}