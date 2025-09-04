using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class Score_UI : MonoBehaviour
{
    private TMPro.TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateScoreText();
        PlayerData.OnScoreChanged += UpdateScoreText;
    }

    private void OnDisable()
    {
        PlayerData.OnScoreChanged -= UpdateScoreText;
    }

    private void UpdateScoreText()
    {
        _scoreText.text = PlayerData.Score.ToString();
    }
}
