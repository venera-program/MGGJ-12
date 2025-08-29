using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy_Spawner : MonoBehaviour
{
    public const float TICK_LENGTH = 0.1f;

    private static Enemy_Spawner Instance;

    public Dictionary<int, SpawnInfo[]> spawnInfos = new();
    public GameObject[] Prefabs;
    public Transform[] Spawners;

    private bool _spawning;
    private int _currentTick;

    public class SpawnInfo
    {
        public SpawnInfo(byte enemyPrefabIndex, byte spawnLocationIndex)
        {
            EnemyPrefabIndex = enemyPrefabIndex;
            SpawnLocationIndex = spawnLocationIndex;
        }

        public byte EnemyPrefabIndex;
        public byte SpawnLocationIndex;
    }

    private void Awake()
    {
        Instance = this;
    }

    public static void LoadProcessFromAsset(TextAsset spawnInfoCSV)
    {
        string[] lines = spawnInfoCSV.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');

            int.TryParse(parts[0], out int tickIndex);
            byte.TryParse(parts[1], out byte enemyPrefabIndex);
            byte.TryParse(parts[2], out byte spawnLocationIndex);

            Instance.spawnInfos[tickIndex].Append(new SpawnInfo(enemyPrefabIndex, spawnLocationIndex));
        }
    }

    public static void StartProcess()
    {
        Instance._spawning = true;
        Instance.StartCoroutine(Instance.ProcessTicks());
    }

    public void EndProcess()
    {
        _spawning = false;
    }

    private IEnumerator ProcessTicks()
    {
        _currentTick = 0;

        while (_spawning)
        {
            foreach (var enemy in spawnInfos[_currentTick])
            {
                // TODO: Spawn enemy from pool
            }
            yield return new WaitForSeconds(TICK_LENGTH); // Wait tick length
            _currentTick++;
        }
    }
}