using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Enemy_Spawner : MonoBehaviour
{
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

    public const float TICK_LENGTH = 0.1f;

    public static Enemy_Spawner Instance;

    public Dictionary<int, List<SpawnInfo>> spawnInfos = new Dictionary<int, List<SpawnInfo>>();
    public GameObject[] EnemyPrefabs;
    public Transform[] EnemySpawners;
    public UnityEvent BossSpawned = new UnityEvent();

    private List<GameObject> _Enemies = new List<GameObject>();
    private int _LatestTickIndex;
    private Coroutine _TickProcess;
    private int _CurrentTick;
    private bool isTicking = true;

    private void Awake()
    {
        Instance = this;

        LevelManager.OnLevelUnload += EndProcess;
    }

    public void StartProcessFromAsset(TextAsset spawnInfoCSV)
    {
        spawnInfos.Clear();

        string[] lines = spawnInfoCSV.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');

            int.TryParse(parts[0], out int tickIndex);
            byte.TryParse(parts[1], out byte enemyPrefabIndex);
            byte.TryParse(parts[2], out byte spawnLocationIndex);

            if (!spawnInfos.ContainsKey(tickIndex))
            {
                spawnInfos[tickIndex] = new List<SpawnInfo>();
                if (tickIndex > _LatestTickIndex)
                {
                    _LatestTickIndex = tickIndex;
                }
            }
            spawnInfos[tickIndex].Add(new SpawnInfo(enemyPrefabIndex, spawnLocationIndex));
        }

        _TickProcess = StartCoroutine(ProcessTicks());
    }

    public void EndProcess()
    {
        if (_TickProcess != null)
        {
            StopCoroutine(_TickProcess);
        }

        GameObject obj;
        for (int i = 0; i < Instance._Enemies.Count; i++)
        {
            obj = Instance._Enemies[i];
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    private IEnumerator ProcessTicks()
    { 
        // put in a bool statement to pause
        _CurrentTick = 0;
        DebugMethods.PrintTicks(_CurrentTick);
        while (_LatestTickIndex > _CurrentTick)
        {
            if(isTicking){
                // Debug.Log("Current Tick is: " + _CurrentTick);
                if (Instance.spawnInfos.ContainsKey(_CurrentTick))
                {
                    foreach (var enemyInfo in spawnInfos[_CurrentTick])
                    {
                        GameObject enemy = Instantiate(EnemyPrefabs[enemyInfo.EnemyPrefabIndex], EnemySpawners[enemyInfo.SpawnerIndex]);
                        _Enemies.Add(enemy);
                        if (enemy.CompareTag("Boss"))
                        {
                            BossSpawned.Invoke();
                        }
                    // Debug.Log("spawned enemy", enemy);
                    }   
                }}

                yield return new WaitForSeconds(TICK_LENGTH); // Wait tick length
                if(isTicking){
                    _CurrentTick++;
                }
                DebugMethods.PrintTicks(_CurrentTick);
            
            
        }

        Debug.Log("Done Spawning.");
    }

    public void TurnOffTick(){
        isTicking = false;
    }

    public void TurnOnTick(){
        isTicking = true;
    }
}