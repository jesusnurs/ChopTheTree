using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LogObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> leftObstacles;
    [SerializeField] private List<GameObject> rightObstacles;
    
    public int strength;
    public LogType logType;
    
    private GameObject currentObstacle;

    private void Awake()
    {
        for (int i = 0; i < leftObstacles.Count; i++)
        {
            leftObstacles[i].SetActive(false);
            rightObstacles[i].SetActive(false);
        }
    }

    public void ChopLog(LogType fromSide)
    {
        //transform.DORotate(-90f * Vector3.one, 0.2f).SetEase(Ease.InQuad);
        //transform.DOMoveY(fromSide == LogType.Left ? 2 : -2, 0.2f).SetEase(Ease.InQuad);
        //transform.DOMoveX(fromSide == LogType.Left ? 20 : -20, 0.2f).SetEase(Ease.InQuad);
    }

    public void SetLogType(LogType _logType, int _strength = 0)
    {
        logType = _logType;
        strength = _strength;
        
        currentObstacle?.SetActive(false);
        
        if (logType == LogType.Left)
        {
            currentObstacle = leftObstacles[Random.Range(0, leftObstacles.Count)];
        }
        else if (logType == LogType.Right)
        {
            currentObstacle = rightObstacles[Random.Range(0, rightObstacles.Count)];
        }
        else
        {
            currentObstacle = null;
        }
        currentObstacle?.SetActive(true);
    }
}

public enum LogType
{
    None,
    Left,
    Right
}