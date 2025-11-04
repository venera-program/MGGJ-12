using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName="Level Dialogue", menuName="Dialogue/Level Dialogue")]
public class LevelDialogueSO : ScriptableObject{
    public DialogueSO startOfLevelDialogue {get => _startOfLevelDialogue;}
    public SerializedDictionary<int, DialogueSO> duringLevelDialogue {get => _duringLevelDialogue;}
    public DialogueSO bossDefeatedDialogue {get => _bossDefeatedDialogue;}

    [SerializeField] private DialogueSO _startOfLevelDialogue;
    [SerializeField] public SerializedDictionary<int, DialogueSO> _duringLevelDialogue;
    [SerializeField] private DialogueSO _bossDefeatedDialogue;
}