using UnityEngine;

public class TestDialogue : MonoBehaviour {

    public DialogueSO sample;
    void OnEnable(){
        DialogueManager.instance.SetUpDialogue(sample);
    }
}