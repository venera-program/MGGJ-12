using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Enemy_Spawner : MonoBehaviour
{
    public const float TICK_LENGTH = 0.1f;

    public static Enemy_Spawner Instance;

    public Dictionary<int, List<SpawnInfo>> spawnInfos = new Dictionary<int, List<SpawnInfo>>();
    public GameObject[] EnemyPrefabs;
    public Transform[] EnemySpawners;

    private List<GameObject> _enemies = new List<GameObject>();

    private bool _spawning;
    private int _currentTick;
    public UnityEvent bossSpawned = new UnityEvent();
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

        LevelManager.OnLevelUnload += EndProcess;
    }

    public static void StartProcessFromAsset(TextAsset spawnInfoCSV)
    {
        Instance.spawnInfos.Clear();

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
        GameObject obj;
        for (int i = 0; i < Instance._enemies.Count; i++)
        {
            obj = Instance._enemies[i];
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        Instance._spawning = false;
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
                    GameObject enemy = Instantiate(EnemyPrefabs[enemyInfo.EnemyPrefabIndex], EnemySpawners[enemyInfo.SpawnerIndex]);
                    _enemies.Add(enemy);
                    if(enemy.tag == "Boss"){
                        bossSpawned.Invoke();
                    }
                }
            }
            yield return new WaitForSeconds(TICK_LENGTH); // Wait tick length
            _currentTick++;
        }
    }
}