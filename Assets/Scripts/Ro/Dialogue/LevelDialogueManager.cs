using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDialogueManager : MonoBehaviour {
    private LevelDialogueSO levelDialogue;
    public static LevelDialogueManager instance;
    
    public void Awake(){
        if(instance != this && instance != null){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    public void Start(){
        LevelClock.Instance.tickTock.AddListener(LoadLevelDialogue);
    }
        LevelClock.Instance.tickTock.RemoveListener(LoadLevelDialogue);
    }
    public void SetCurrLevelDialogue(LevelDialogueSO leveldialogue){
        levelDialogue = leveldialogue;
    }

    public void LoadStartDialogue(){
        if(levelDialogue != null){
            if(levelDialogue.startOfLevelDialogue != null){
                DialogueManager.instance.SetUpDialogue(levelDialogue.startOfLevelDialogue);
            }
        }
    }
    public void LoadLevelDialogue(int tick){
        if(levelDialogue!= null){
            if (levelDialogue.duringLevelDialogue.ContainsKey(tick)){
                DialogueManager.instance.SetUpDialogue(levelDialogue.duringLevelDialogue[tick]);
            }
        }
        
    }

    public void StartBossDefeatedDialogue(){
        if(levelDialogue != null){
            if(levelDialogue.bossDefeatedDialogue != null){
                levelDialogue.bossDefeatedDialogue.dialogueEnd = LevelManager.ResumeLevelTransition;
                DialogueManager.instance.SetUpDialogue(levelDialogue.bossDefeatedDialogue);
            }
        }
    }

   
}