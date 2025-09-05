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

        if (CurrentLevelIndex == -1)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.StopAllSfx();
            MenuUI.SetActive(true);
            // The main menu doesn't need an execution loop
            return;
        }

        _levelThread = StartCoroutine(ExecuteLevel());
    }

    private IEnumerator ExecuteLevel()
    {
        CombatUI.SetActive(true);
        BackgroundImage.sprite = levels[CurrentLevelIndex].NewBackgroundTexture;
        if(AudioManager.Instance != null){
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.StopAllSfx();
            AudioManager.Instance.PlayMusic(levels[CurrentLevelIndex].bgMusic);
        }
        BackgroundUI.SetActive(true);
        Enemy_Spawner.StartProcessFromAsset(levels[CurrentLevelIndex].SpawnInfoCSV);

        BossDefeated = false;
        while (!BossDefeated)
        {
            yield return null;
        }

        if(levels.Length <= CurrentLevelIndex + 1){
            yield return new WaitForSeconds(winScreenDelay);
            Debug.Log("should be opening the win screen");
            OnGameWin.Invoke();
        }

        yield return new WaitForSeconds(levelLoadDelay);


        // Load next level, else load main menu
        // L 1|2|3
        // i 0|1|2
        if (levels.Length > CurrentLevelIndex + 1)
        {
            UnloadLevel();
            LoadLevel((sbyte)(CurrentLevelIndex + 1));
        }
    }

    private void UnloadLevel()
    {
        BackgroundUI.SetActive(false);
        MenuUI.SetActive(false);
        CombatUI.SetActive(false);
        OnLevelUnload.Invoke();
    }

    [ContextMenu("LoadMainMenu")]
    public void LoadMainMenu()
    {
        if(PlayerControllerScript.instance != null){
            PlayerControllerScript.instance.DisablePlayerControls();
            Debug.Log("Freezing Game");
        } 
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
        UnloadLevel();
        LoadLevel(CurrentLevelIndex);
    }
}