using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class LevelClock : MonoBehaviour {
    public static LevelClock Instance;
    public const float TICK_LENGTH = 0.1f;
    private int _LatestTickIndex = 1000;
    private Coroutine _TickProcess;
    private int _CurrentTick;
    private bool isTicking = true;
    public UnityEvent startOfClock = new UnityEvent();
    public UnityEvent<int> tickTock = new UnityEvent<int>();
    public UnityEvent endOfClock = new UnityEvent();

    public void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start(){
        LevelManager.OnLevelUnload += StopClock;
    }

    public void StartClock(){
        _TickProcess = StartCoroutine(ProcessTicks());
    }

    public void StopClock(){
        if (_TickProcess != null){
            StopCoroutine(_TickProcess);
        }
        endOfClock.Invoke();
    }

     private IEnumerator ProcessTicks(){ 
        _CurrentTick = 0;
        while (_LatestTickIndex > _CurrentTick){
            DebugMethods.PrintTicks(_CurrentTick);
            yield return new WaitForSeconds(TICK_LENGTH);
            
            if(isTicking){
                tickTock.Invoke(_CurrentTick);
                _CurrentTick++;
            } 
        }
    }

    public void TurnOffTick(){
        isTicking = false;
    }

    public void TurnOnTick(){
        isTicking = true;
    }


}