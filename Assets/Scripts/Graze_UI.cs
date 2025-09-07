using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class Graze_UI : MonoBehaviour
{
    private TMPro.TMP_Text _grazeText;

    private void Awake()
    {
        _grazeText = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateGrazeText();
        PlayerData.OnGrazeChanged += UpdateGrazeText;
    }

    private void OnDisable()
    {
        PlayerData.OnGrazeChanged -= UpdateGrazeText;
    }

    private void UpdateGrazeText()
    {
        _grazeText.text = PlayerData.Graze.ToString();
    }
}
