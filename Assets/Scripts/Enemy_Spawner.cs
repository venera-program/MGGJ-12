using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy_Spawner : MonoBehaviour
{
    public const float TICK_LENGTH = 0.1f;

    public static Enemy_Spawner Instance;

    public Dictionary<int, List<SpawnInfo>> spawnInfos = new Dictionary<int, List<SpawnInfo>>();
    public GameObject[] EnemyPrefabs;
    public Transform[] EnemySpawners;

    private bool _spawning;
    private int _currentTick;

    public class SpawnInfo
    {
        public SpawnInfo(byte enemyPrefabIndex, byte spawnLocationIndex)
        {
            EnemyPrefabIndex = enemyPrefabIndex;
            SpawnerIndex = spawnLocationIndex;
        }

        public byte EnemyPrefabIndex;
        public byte SpawnerIndex;
    }

    private void Awake()
    {
        Instance = this;
    }

    public static void StartProcessFromAsset(TextAsset spawnInfoCSV)
    {
        string[] lines = spawnInfoCSV.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');

            int.TryParse(parts[0], out int tickIndex);
            byte.TryParse(parts[1], out byte enemyPrefabIndex);
            byte.TryParse(parts[2], out byte spawnLocationIndex);

            if (!Instance.spawnInfos.ContainsKey(tickIndex))
            {
                Instance.spawnInfos[tickIndex] = new List<SpawnInfo>();
            }
            Instance.spawnInfos[tickIndex].Add(new SpawnInfo(enemyPrefabIndex, spawnLocationIndex));
        }

        Instance._spawning = true;
        Instance.StartCoroutine(Instance.ProcessTicks());
    }

    public static void EndProcess()
    {
        Instance._spawning = false;
    }

    public void ClearEnemies(){
        foreach(Transform t in EnemySpawners){
            Transform[] childEnemies = t.GetComponentsInChildren<Transform>();
            for(int i = 0; i < childEnemies.Length ; i++){
                if(childEnemies[i] == t){continue;}
                Destroy(childEnemies[i].gameObject);
            }
        }
    }

    private IEnumerator ProcessTicks()
    {
        _currentTick = 0;

        while (_spawning)
        {
            if (Instance.spawnInfos.ContainsKey(_currentTick))
            {
                foreach (var enemyInfo in spawnInfos[_currentTick])
                {
                    Instantiate(EnemyPrefabs[enemyInfo.EnemyPrefabIndex], EnemySpawners[enemyInfo.SpawnerIndex]);
                }
            }
            yield return new WaitForSeconds(TICK_LENGTH); // Wait tick length
            _currentTick++;
        }
    }
}