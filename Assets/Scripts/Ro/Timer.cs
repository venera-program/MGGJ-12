using System;

public struct Timer {
    private float interval;
    private float delay;
    private bool pastDelay;
    private float currTime;

    public void Initialize(float _interval, float _delay){
        ClearSettings();
        interval = _interval;
        delay = _delay;
    }

    public float CurrentTime(){
        return currTime;
    }
    private void ClearSettings(){
        interval = 0f;
        delay = 0f;
        pastDelay = false;
        currTime = 0f;
    }

    public void ResetTimer(){
        currTime = 0f;
    }

    public void Update(float delataTime){
        currTime += delataTime;
    }

    public bool isTimerDone(){
        bool timerDone = false;
        if(!pastDelay){
            pastDelay = currTime >= delay;
            timerDone = pastDelay;
        } else {
            timerDone = currTime >= interval;
        }
        return timerDone; 
    }
}