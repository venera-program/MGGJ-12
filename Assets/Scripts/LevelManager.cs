using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static Action<sbyte> OnLevelChange = _ => { };

    public static sbyte CurrentLevelIndex { get; private set; }

    public GameObject MenuUI;
    public GameObject CombatUI;
    public Image BackgroundUI;

    public LevelInfo[] levels = { };

    private Coroutine _levelThread;

    void Start(){
        LoadMainMenu();
    }

    private void OnDestroy()
    {
        if (_levelThread != null)
        {
            StopCoroutine(_levelThread);
        }
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
            MenuUI.SetActive(true);
            // UI: Enable main menu
            return;
        }

        _levelThread = StartCoroutine(ExecuteLevel());
    }

    private IEnumerator ExecuteLevel()
    {
        CombatUI.SetActive(true);
        BackgroundUI.sprite = levels[CurrentLevelIndex].NewBackgroundTexture;
        BackgroundUI.enabled = true;
        Enemy_Spawner.StartProcessFromAsset(levels[CurrentLevelIndex].SpawnInfoCSV);

        // Wait for level to be over
        var levelComplete = false;
        while (!levelComplete)
        {
            yield return new WaitForSeconds(60); // TODO: Level completion flag (not timer)
            levelComplete = true;
        }

        UnloadLevel();

        // TODO: Figure out which level to load
        LoadMainMenu();
    }

    private void UnloadLevel()
    {
        BackgroundUI.enabled = false;
        MenuUI.SetActive(false);
        CombatUI.SetActive(false);
        Enemy_Spawner.EndProcess();
        if(Enemy_Spawner.Instance != null){
            Enemy_Spawner.Instance.ClearEnemies();
        }
    }

    [ContextMenu("LoadMainMenu")]
    public void LoadMainMenu()
    {
        if(PlayerControllerScript.instance != null){
            PlayerControllerScript.instance.DisablePlayerControls();}
        UnloadLevel();
        LoadLevel(-1);
    }

    [ContextMenu("LoadFirstLevel")]
    public void LoadFirstLevel()
    {
        if(PlayerControllerScript.instance != null){
            PlayerControllerScript.instance.EnablePlayerControls();
        }
        UnloadLevel();
        LoadLevel(0);
    }

    public void RestartLevel(){
        if(PlayerControllerScript.instance != null){
            PlayerControllerScript.instance.EnablePlayerControls();
        }
        UnloadLevel();
        LoadLevel(CurrentLevelIndex);
    }
}