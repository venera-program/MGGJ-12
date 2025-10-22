using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour{
    public static DialogueManager instance;
    [SerializeField] private GameObject dialogueMenu;
    [SerializeField] private TMP_Text textBox;
    private DialogueSO currDialogue;
    private int currIndex;

    public void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

   public void SetUpDialogue(DialogueSO newDialogue){
        PlayerControllerScript.instance.DisablePlayerControls();
        dialogueMenu.SetActive(true);
        currDialogue = newDialogue;
        currIndex = 0;
        GameManager.instance.StopTick();
        GameManager.instance.StopEnemy();
        GameManager.instance.StopPlayerAnimation();
        GameManager.instance.StopGrazeDetection();
        GameManager.instance.StopSkillTimer();
        GameManager.instance.StopProjectileMovement();
        ProgressDialogue();
   }

   public void ProgressDialogue(){
        if(currIndex >= currDialogue.dialogue.Length) {
            EndDialogue();
        }
        textBox.text = currDialogue.dialogue[currIndex];
        currIndex++;
   }

   public void EndDialogue(){
        textBox.text = "";
        currIndex = 0;
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
