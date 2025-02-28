using System;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    
    [Space]
    [SerializeField] private float maxTime;
    [SerializeField] private float timeForChoppedLog;

    public Action OnTimerUpdated;
    
    private float currentTime;
    private bool isTimerActive;

    private void Awake()
    {
        stateMachine.OnGameStarted += ResetTimer;
        stateMachine.OnGameContinue += ResetTimer;
        stateMachine.OnChopLog += StartTimer;
        stateMachine.OnGameEnded += StopTimer;
    }

    private void Update()
    {
        if(!isTimerActive)
            return;
        
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        OnTimerUpdated?.Invoke();

        if (currentTime <= 0)
        {
            TimeOver();
        }
    }

    private void TimeOver()
    {
        stateMachine.EndGame();
        stateMachine.OnTimerEnd?.Invoke();
    }

    private void StartTimer()
    {
        if(isTimerActive)
            return;
        
        isTimerActive = true;
    }

    private void StopTimer()
    {
        isTimerActive = false;
    }
    
    private void ResetTimer()
    {
        currentTime = maxTime;
        OnTimerUpdated?.Invoke();
    }

    public void IncreaseTime()
    {
        currentTime += timeForChoppedLog;
        if(currentTime > maxTime)
            currentTime = maxTime;
    }
    
    public float GetCurrentTimeRatio()
    {
        return currentTime / maxTime;
    }
}
