using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private TimerController timerController;

    [Space] 
    [SerializeField] private GameObject simpleLogPrefab;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float logHeight;
    [SerializeField] private int logsCount;

    private LogObject[] treeLogs;

    private int simpleLogToSpawn;

    private void Start()
    {
        GenerateTree();
    }

    private void GenerateTree()
    {
        treeLogs = new LogObject[logsCount];

        for (int i = 0; i < 3; i++)
        {
            treeLogs[i] = Instantiate(simpleLogPrefab, transform).GetComponent<LogObject>();
            GenerateSimpleLog(i);
        }
        simpleLogToSpawn = 0;

        for (int i = 3; i < logsCount; i++)
        {
            if(simpleLogToSpawn > 0)
            {
                treeLogs[i] = Instantiate(simpleLogPrefab, transform).GetComponent<LogObject>();
                simpleLogToSpawn -= Random.Range(1,3);
                continue;
            }
            treeLogs[i] = Instantiate(simpleLogPrefab, transform).GetComponent<LogObject>();
            GenerateRandomLog(i);
            simpleLogToSpawn = 2;
        }
    }

    /// <param name="logType">Which side player choped</param>
    public bool TryChopLog(LogType choppedSide)
    {
        if (choppedSide == treeLogs[0].logType)
        {
            stateMachine.EndGame();
            return false;
        }

        if (treeLogs[0].strength > 1)
        {
            treeLogs[0].strength -= 1;
            return true;
        }

        ChopLog(choppedSide);
        return true;
    }

    private void ChopLog(LogType choppedSide)
    {
        treeLogs[0].ChopLog(choppedSide);
        particles.Play();
        AudioManager.Instance.PlaySound(AudioClipEnum.ChopLog);
        stateMachine.ChopLog();
        
        var choppedLog = treeLogs[0];

        for (int i = 0; i < logsCount; i++)
        {
            if (i == logsCount - 1)
            {
                treeLogs[i] = choppedLog;
                if(simpleLogToSpawn > 0)
                {
                    GenerateSimpleLog(i);
                    simpleLogToSpawn -= Random.Range(1,3);
                    break;
                }
                GenerateRandomLog(i);
                simpleLogToSpawn = 2;
                break;
            }

            treeLogs[i] = treeLogs[i + 1];
            treeLogs[i].transform.DOMoveY(logHeight * i, 0.1f).SetEase(Ease.InQuad);
        }

        if (choppedSide == treeLogs[0].logType)
        {
            stateMachine.EndGame();
            return;
        }
        
        timerController.IncreaseTime();
    }


    private LogType logType;
    private void GenerateRandomLog(int index)
    {
        int randomIndex = Random.Range(0,11);
        if (randomIndex == 0)
            logType = LogType.None;
        else if (randomIndex % 2 == 0)
            logType = LogType.Left;
        else 
            logType = LogType.Right;
        
        treeLogs[index].SetLogType(logType);
        treeLogs[index].transform.position = new Vector3(0, logHeight * index, 0);
    }

    private void GenerateSimpleLog(int index)
    {
        treeLogs[index].SetLogType(LogType.None);
        treeLogs[index].transform.position = new Vector3(0, logHeight * index, 0);
    }
}