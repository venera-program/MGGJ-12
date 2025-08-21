using System;
using UnityEngine;

public static class PlayerData
{
    //
    public static Action OnScoreChanged = () => { };
    public static float Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged();
        }
    }
    [SerializeField] private static float _score = 0f;

    //
    public static Action OnLivesChanged = () => { };
    public static byte LivesRemaining
    {
        get => _livesRemaining;
        set
        {
            _livesRemaining = value;
            OnLivesChanged();
        }
    }
    [SerializeField] private static byte _livesRemaining;

    // Methods
    public static void UpdateScore(float additionalAmount)
    {
        _score += additionalAmount;
        Debug.Log($"New Score: {_score}");
    }
}
