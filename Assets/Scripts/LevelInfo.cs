using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level Asset")]
public class LevelInfo : ScriptableObject
{
    public Sprite NewBackgroundTexture { get => _newBackgroundTexture; }
    public TextAsset SpawnInfoCSV { get => _spawnInfoCSV; }
    public AudioClip BGMusic { get => _bgMusic; }
    public LevelDialogueSO LevelDialogue {get => _levelDialogue;}

    [SerializeField] private Sprite _newBackgroundTexture;
    [SerializeField] private TextAsset _spawnInfoCSV;
    [SerializeField] private AudioClip _bgMusic;
    [SerializeField] private LevelDialogueSO _levelDialogue;
}