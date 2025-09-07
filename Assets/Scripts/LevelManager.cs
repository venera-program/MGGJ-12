using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MGGJ25.Shared;

public class LevelManager : MonoBehaviour
{
    public static Action<sbyte> OnLevelChange = (_) => { };
    public static Action OnLevelUnload = () => { };

    public static sbyte CurrentLevelIndex { get; private set; }
    public static bool BossDefeated = false;

    public static Action OnGameWin = () => { };

    public GameObject MenuUI;
    public GameObject CombatUI;
    public GameObject BackgroundUI;
    public Image BackgroundImage;

    public LevelInfo[] levels = { };

    private Coroutine _levelThread;

    public float levelLoadDelay;
    public float winScreenDelay;

    void Start()
    {
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

        if (_levelThread != null)
        {
            Debug.LogWarning("Stopping existing level thread.");
            StopCoroutine(_levelThread);
            _levelThread = null;
        }

        if (CurrentLevelIndex == -1)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopClearMusic();
                AudioManager.Instance.StopAllSfx();
            }
            MenuUI.SetActive(true);
            // The main menu doesn't need an execution loop
            return;
        }

        _levelThread = StartCoroutine(ExecuteLevel());
    }

    private IEnumerator ExecuteLevel()
    {
        SetUpLevel();
        while (!BossDefeated)
        {
            yield return null;
        }
        Debug.Log($"Boss has been defeated. Current Level Index is {CurrentLevelIndex}");
        yield return new WaitForSeconds(levelLoadDelay);
        Debug.Log($"2. Boss has been defeated. Current Level Index is {CurrentLevelIndex}");

        // Load next level, else load win screen
        // L 1|2|3
        // i 0|1|2
        if (levels.Length > CurrentLevelIndex + 1)
        {
            UnloadLevel();
            if (PlayerControllerScript.instance != null)
            {
                PlayerControllerScript.instance.EnablePauseButton();
            }
            LoadLevel((sbyte)(CurrentLevelIndex + 1));
        }
        else
        {
            yield return new WaitForSeconds(winScreenDelay);
            OnGameWin.Invoke();
            if (PlayerControllerScript.instance != null)
            {
                PlayerControllerScript.instance.DisablePauseButton();
            }
        }
    }

    private void UnloadLevel()
    {
        BackgroundUI.SetActive(false);
        MenuUI.SetActive(false);
        CombatUI.SetActive(false);
        OnLevelUnload.Invoke();
    }

    private void SetUpLevel()
    {
        CombatUI.SetActive(true);
        BackgroundImage.sprite = levels[CurrentLevelIndex].NewBackgroundTexture;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopClearMusic();
            AudioManager.Instance.StopAllSfx();
            AudioManager.Instance.PlayMusic(levels[CurrentLevelIndex].bgMusic);
        }
        BackgroundUI.SetActive(true);
        Enemy_Spawner.Instance.StartProcessFromAsset(levels[CurrentLevelIndex].SpawnInfoCSV);
        BossDefeated = false;
    }

    [ContextMenu("LoadMainMenu")]
    public void LoadMainMenu()
    {
        if (PlayerControllerScript.instance != null)
        {
            PlayerControllerScript.instance.DisablePlayerControls();
            Debug.Log("Freezing Game");
        }
        UnloadLevel();
        LoadLevel(-1);
    }

    [ContextMenu("LoadFirstLevel")]
    public void LoadFirstLevel()
    {
        if (PlayerControllerScript.instance != null)
        {
            PlayerControllerScript.instance.EnablePlayerControls();
        }
        UnloadLevel();
        LoadLevel(0);
    }

    public void RestartLevel()
    {
        UnloadLevel();
        LoadLevel(CurrentLevelIndex);
    }
}