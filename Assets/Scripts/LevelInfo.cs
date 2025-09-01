using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level Asset")]
public class LevelInfo : ScriptableObject
{
    public Sprite NewBackgroundTexture { get => _newBackgroundTexture; }
    public TextAsset SpawnInfoCSV { get => _spawnInfoCSV; }

    [SerializeField] private Sprite _newBackgroundTexture;
    [SerializeField] private TextAsset _spawnInfoCSV;
}