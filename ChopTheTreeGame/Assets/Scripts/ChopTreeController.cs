using System;
using UnityEngine;
using UnityEngine.UI;

public class ChopTreeController : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private TreeGenerator treeGenerator;

    [Space] 
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    [Header("Lumberjack")]
    [SerializeField] private Transform lumberjackTransform;
    
    [Header("Lumberjack's position")]
    [SerializeField] private Transform lumberjackLeftTransform;
    [SerializeField] private Transform lumberjackRightTransform;

    private Animator lumberjackAnimator;

    private bool isTimeOver;
    private bool inputEnabled;
    private float screenMiddle;
    
    private LogType lumberjackLogTypeCanBeKilled;

    private void Awake()
    {
        lumberjackAnimator = lumberjackTransform.GetComponent<Animator>();

        stateMachine.OnMainMenuOpened += DisableInput;
        stateMachine.OnGameEnded += DisableInput;
        stateMachine.OnGameContinue += EnableInput;
        stateMachine.OnGameStarted += EnableInput;

        stateMachine.OnMainMenuOpened += StandUpAnim;
        stateMachine.OnChopLog += ChopLogAnim;
        stateMachine.OnGameContinue += StandUpAnim;
        stateMachine.OnGameEnded += DeathAnim;
        
        stateMachine.OnTimerEnd += () => { isTimeOver = true; };
        
        //leftButton.onClick.AddListener(ChopLogFromLeft);
        //rightButton.onClick.AddListener(ChopLogFromRight);

        screenMiddle = Screen.width / 2;
    }

    private void Start()
    {
        IdleAnim();
    }

    private void Update()
    {
        if (!inputEnabled)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch(Input.mousePosition);
        }
    }
    
    private void HandleTouch(Vector2 touchPosition)
    {
        if (touchPosition.x < screenMiddle)
        {
            ChopLogFromLeft();
        }
        else
        {
            ChopLogFromRight();
        }
    }

    private void EnableInput()
    {
        inputEnabled = true;
        leftButton.interactable = true;
        rightButton.interactable = true;
    }
    
    private void DisableInput()
    {
        inputEnabled = false;
        leftButton.interactable = false;
        rightButton.interactable = false;
    }

    private void IdleAnim()
    {
        lumberjackAnimator.SetTrigger("Idle");
    }

    private void StandUpAnim()
    {
        lumberjackAnimator.ResetTrigger("Attack");
        lumberjackAnimator.SetTrigger("StandUp");
        ReviewLumberjack();
        isTimeOver = false;
    }
    
    private void ChopLogAnim()
    {
        lumberjackAnimator.SetTrigger("Attack");
    }

    private void DeathAnim()
    {
        lumberjackAnimator.ResetTrigger("StandUp");
        lumberjackAnimator.SetTrigger("Death");
    }

    public void ChopLogFromLeft()
    {
        treeGenerator.TryChopLog(LogType.Left);
        
        lumberjackTransform.position = lumberjackLeftTransform.position;
        lumberjackTransform.rotation = lumberjackLeftTransform.rotation;
        var scale = lumberjackTransform.localScale;
        scale.x = Math.Abs(scale.x);
        lumberjackTransform.localScale = scale;

        lumberjackLogTypeCanBeKilled = LogType.Left;
    }
    
    public void ChopLogFromRight()
    {
        treeGenerator.TryChopLog(LogType.Right);
        
        lumberjackTransform.position = lumberjackRightTransform.position;
        lumberjackTransform.rotation = lumberjackRightTransform.rotation;
        var scale = lumberjackTransform.localScale;
        scale.x = Math.Abs(scale.x) * -1;
        lumberjackTransform.localScale = scale;

        lumberjackLogTypeCanBeKilled = LogType.Right;
    }

    private void ReviewLumberjack()
    {
        if(isTimeOver)
            return;
        
        if (lumberjackLogTypeCanBeKilled == LogType.Left)
        {
            lumberjackTransform.position = lumberjackRightTransform.position;
            lumberjackTransform.rotation = lumberjackRightTransform.rotation;
            var scale = lumberjackTransform.localScale;
            scale.x = Math.Abs(scale.x) * -1;
            lumberjackTransform.localScale = scale;
            return;
        }

        if (lumberjackLogTypeCanBeKilled == LogType.Right)
        {
            lumberjackTransform.position = lumberjackLeftTransform.position;
            lumberjackTransform.rotation = lumberjackLeftTransform.rotation;
            var scale = lumberjackTransform.localScale;
            scale.x = Math.Abs(scale.x);
            lumberjackTransform.localScale = scale;
        }
    }
}
