using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Action OnGameStarted;
    public Action OnGameContinue;
    public Action OnGameEnded;
    public Action OnTimerEnd;
    public Action OnMainMenuOpened;
    public Action OnChopLog;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        OnMainMenuOpened?.Invoke();
    }
    
    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void ContinueGame()
    {
        OnGameContinue?.Invoke();
    }
    
    public void EndGame()
    {
        OnGameEnded?.Invoke();
    }

    public void ChopLog()
    {
        OnChopLog?.Invoke();
    }
}
