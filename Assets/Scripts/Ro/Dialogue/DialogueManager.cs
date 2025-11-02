using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour{
    public static DialogueManager instance;
    [SerializeField] private GameObject dialogueMenu;
    [SerializeField] private TMP_Text textBox;
    private DialogueSO currDialogue;
    private int textBoxIndex;
    [SerializeField] private float wordLoadingSeconds = .1f;
    private int wordIndex;
    private Timer loadingTimer;
    private string[] words = {};
    private bool isLoadingText = false;

    public void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public void Start(){
        loadingTimer = new Timer();
        loadingTimer.Initialize(wordLoadingSeconds, wordLoadingSeconds);
    }

    public void Update(){
        if(isLoadingText){
            loadingTimer.Update(Time.deltaTime);
            if(loadingTimer.isTimerDone()){
                loadingTimer.ResetTimer();
                LoadTextBox();
            }
        }   
    }

   public void SetUpDialogue(DialogueSO newDialogue){
        PlayerControllerScript.instance.DisablePlayerControls();
        dialogueMenu.SetActive(true);
        currDialogue = newDialogue;
        textBoxIndex = 0;
        GameManager.instance.StopTick();
        GameManager.instance.StopEnemy();
        GameManager.instance.StopPlayerAnimation();
        GameManager.instance.StopGrazeDetection();
        GameManager.instance.StopSkillTimer();
        GameManager.instance.StopProjectileMovement();
        ProgressDialogue();
   }
    // Called by progress button UI
   public void ProgressDialogue(){
        if(textBoxIndex >= currDialogue.dialogue.Length) {
            EndDialogue();
        }
        if(!isLoadingText){
            words = currDialogue.dialogue[textBoxIndex].Split(" ");
            textBox.text = "";
            wordIndex = 0;
            isLoadingText = true;
        } else {
            isLoadingText = false;
            textBox.text = currDialogue.dialogue[textBoxIndex];
            wordIndex = 0;
            textBoxIndex++;
        }
   }

   private void LoadTextBox(){
        if(wordIndex >= words.Length) {
            isLoadingText = false;
            textBoxIndex++;
            return;
        }

        if(wordIndex > 0){
            textBox.text += (" " + words[wordIndex]); 
        } else {
            textBox.text += words[wordIndex]; 
        }
        wordIndex++;
   }

   private void EndTextBox(){

   }

   private void EndDialogue(){
        textBox.text = "";
        textBoxIndex = 0;
        dialogueMenu.SetActive(false);
        PlayerControllerScript.instance.EnablePlayerControls();
        PlayerControllerScript.instance.EnablePauseButton();
        GameManager.instance.StartTick();
        GameManager.instance.StartEnemy();
        GameManager.instance.StartPlayerAnimation();
        GameManager.instance.StartGrazeDetection();
        GameManager.instance.StartSkillTimer();
        GameManager.instance.StartProjectileMovement();
   }
}
