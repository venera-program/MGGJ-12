using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDialogueManager : MonoBehaviour {
    private SerializedDictionary<int, DialogueSO> currLevelDialogue;
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
    public void SetCurrLevelDialogue(SerializedDictionary<int, DialogueSO> current){
        currLevelDialogue = current;
    }
    public void LoadLevelDialogue(int tick){
        if (currLevelDialogue.ContainsKey(tick)){
            DialogueManager.instance.SetUpDialogue(currLevelDialogue[tick]);
        }
    }
}