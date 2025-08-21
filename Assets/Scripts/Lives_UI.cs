using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class Lives_UI : MonoBehaviour
{
    private TMPro.TMP_Text _livesText;

    private void Awake()
    {
        _livesText = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateLivesText();
        PlayerData.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        PlayerData.OnLivesChanged -= UpdateLivesText;
    }

    private void UpdateLivesText()
    {
        // 3 Lives => "\U0001F600 \U0001F600 \U0001F600" 
        _livesText.text = string.Concat(System.Linq.Enumerable.Repeat("\U0001F600", PlayerData.LivesRemaining));
    }
}
