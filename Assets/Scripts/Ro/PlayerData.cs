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
        Score += additionalAmount;
        Debug.Log($"New Score: {_score}");
    }

    public static void ClearScore()
    {
        Score = 0f;
    }

    public static Action OnGrazeChanged = () => { };
    public static int Graze
    {
        get => _graze;
        set
        {
            _graze = value;
            OnGrazeChanged();
        }
    }
    [SerializeField] private static int _graze = 0;
    public static void AddGraze(int additionalAmount)
    {
        Graze += additionalAmount;
    }

    public static void ClearGraze()
    {
        Graze = 0;
    }
}
