using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action<sbyte> OnLevelChange = _ => { };

    public static sbyte CurrentLevelIndex { get; private set; }

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
    public void LoadLevel(sbyte levelNumber)
    {
        CurrentLevelIndex = levelNumber;
        OnLevelChange.Invoke(CurrentLevelIndex);

        // Level -1 is the main menu.
        if (CurrentLevelIndex == -1)
        {
            // Enable main menu stuff
            return;
        }

        _levelThread = StartCoroutine(ExecuteLevel());
    }

    private IEnumerator ExecuteLevel()
    {
        yield return null;

        // -- You can access most info via `levels[_currentLevel]`

        // TODO: LevelStart Dialogue

        // TODO: UI Initialization
        //  This is the "switching from dialogue UI to gameplay UI or w/e

        Enemy_Spawner.LoadProcessFromAsset(levels[CurrentLevelIndex].SpawnInfoCSV);
        Enemy_Spawner.StartProcess();

        // TODO: Wait for level to be over
        var levelComplete = false;
        while (!levelComplete)
        {
            yield return new WaitForSeconds(20);
            levelComplete = true;
        }

        // TODO: LevelComplete Dialogue

        // -- For now, load the main level when everything is done
        LoadLevel(-1);
    }
}