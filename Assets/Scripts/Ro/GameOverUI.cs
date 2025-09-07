using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private GameObject gameOverUI;

    void Start()
    {
        PlayerControllerScript.instance.GetComponent<Health>().healthChange.AddListener(OpenGameOverUI);
    }

    private void OpenGameOverUI(float currHealth, float maxHealth)
    {
        if (currHealth == 0)
        {
            gameOverUI.SetActive(true);
        }
    }
}