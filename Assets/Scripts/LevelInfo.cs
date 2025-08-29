using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : ScriptableObject
{
    public Sprite NewBackgroundTexture { get => _newBackgroundTexture; set => _newBackgroundTexture = value; }
    public TextAsset SpawnInfoCSV { get => _spawnInfoCSV; set => _spawnInfoCSV = value; }

    [SerializeField] private Sprite _newBackgroundTexture;
    [SerializeField] private TextAsset _spawnInfoCSV;
}