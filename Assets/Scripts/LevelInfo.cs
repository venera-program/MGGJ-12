using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level Asset")]
public class LevelInfo : ScriptableObject
{
    public Sprite NewBackgroundTexture { get => _newBackgroundTexture; }
    public TextAsset SpawnInfoCSV { get => _spawnInfoCSV; }
    public AudioClip bgMusic { get => _bgMusic; }

    [SerializeField] private Sprite _newBackgroundTexture;
    [SerializeField] private TextAsset _spawnInfoCSV;
    [SerializeField] private AudioClip _bgMusic;
}