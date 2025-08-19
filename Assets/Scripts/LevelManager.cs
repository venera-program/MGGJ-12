using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action<int> OnLevelChange = _ => { };

    public static byte CurrentLevel
    {
        get => _currentLevel;
        private set
        {
            _currentLevel = value;
            OnLevelChange.Invoke(_currentLevel);
        }
    }
    private static byte _currentLevel;

    public LevelInfo[] levels = { };

    private Coroutine _levelThread;

    private void OnDestroy()
    {
        StopCoroutine(_levelThread);
    }

    /// <summary>
    /// Loads the level by its number.
    /// </summary>
    /// <param name="levelNumber">The number corresponding the level. LevelNumber = 0 is the main menu.</param>
    public void LoadLevel(byte levelNumber)
    {
        CurrentLevel = levelNumber;

        // Level 0 is the main menu.
        if (CurrentLevel == 0)
        {
            // Enable main menu stuff
            return;
        }

        levels[CurrentLevel].Initialize();
        _levelThread = StartCoroutine(ExecuteLevel());
    }

    private IEnumerator ExecuteLevel()
    {
        yield return null;

        // -- You can access most info via `levels[_currentLevel]`

        // TODO: LevelStart Dialogue

        // TODO: UI Initialization
        //  This is the "switching from dialogue UI to gameplay UI or w/e

        // TODO: Enemy spawning

        // TODO: Wait for level to be over
        // var levelComplete = false;
        // while (!levelComplete)
        // {
        //     yield return null;
        //     levelComplete = true;
        // }

        // TODO: LevelComplete Dialogue

        // -- For now, load the main level when everything is done
        LoadLevel(0);
    }
}