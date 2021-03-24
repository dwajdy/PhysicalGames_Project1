using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    

    enum Action
    {
        Jump,
        Dance,
        Color,
    }

    // ###############
    // ## INTERFACE ##
    // ###############

    public float StartingSpeed = 2;
    public float SpeedChangePercentage = 0.2f;

    public bool DebugMode = false;

    // ###############
    // ## PUBLICS ##
    // ###############

    [HideInInspector]
    public static GameManager Instance = null;
    
    // ##############
    // ## PRIVATES ##
    // ##############
    
    // --------------
    // Game vars
    // --------------
    private const float MinSpeed = 0.5f;

    private bool gameStarted = false;
    private bool gameOver = false;

    private float currentSpeed;

    private uint currentScore = 0;

    private float timePassed = 0;

    private IAction latestAction = null;

    private float healthBarScale = 50.0f;
    private float healthBarScaleChange = 10f;

    private bool isLastActionPressed = true;

    private System.Random random = new System.Random();

    // --------------
    // Game objects
    // --------------
    private GameObject healthBar;
    private GameObject character;
    private GameObject  blueColor;
    private GameObject redColor;
    private GameObject yellowColor;
    private GameObject orangeColor;
    private Button startButton;
    private Text actionText;
    private Text successText;
    private Text scoreText;
    private Text startButtonText;

    void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // setting initial speed
        currentSpeed = StartingSpeed;

        // get game objects
        character = GameObject.FindGameObjectWithTag("Character");
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        blueColor = GameObject.FindGameObjectWithTag("BlueColor");
        redColor = GameObject.FindGameObjectWithTag("RedColor");
        orangeColor = GameObject.FindGameObjectWithTag("OrangeColor");
        yellowColor = GameObject.FindGameObjectWithTag("YellowColor");
        actionText = GameObject.FindGameObjectWithTag("ActionText").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        successText = GameObject.FindGameObjectWithTag("SuccessText").GetComponent<Text>();
        startButtonText = GameObject.FindGameObjectWithTag("StartButtonText").GetComponent<Text>();
        startButton = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();

        // disable not needed objects
        successText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startButton.IsActive())
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                onClick();
            }
        }

        if(gameOver)
        {
            if (DebugMode)
            {
                Debug.Log("Game over.");
            }
            actionText.text = "GAME OVER!";
            startButton.gameObject.SetActive(true);

            return;
        }

        int numOfActions = Enum.GetNames(typeof(Action)).Length;

        if(gameStarted)
        {
            timePassed += Time.deltaTime;
            if(timePassed > currentSpeed)
            {
                successText.gameObject.SetActive(false);

                if(!isLastActionPressed)
                {
                    healthBarScale -= healthBarScaleChange;

                    healthBar.transform.localScale = new Vector3(healthBarScale, healthBar.transform.localScale.y);

                    if(healthBarScale <= 0)
                    {
                        gameOver = true;
                        return;
                    }
                }
                isLastActionPressed = false; //reset action pressed.

                Action randomAction = (Action) random.Next(0, numOfActions);

                IAction newAction;
                switch(randomAction)
                {
                    case Action.Jump:
                        newAction = new Jump();
                        break;
                    case Action.Dance:
                        newAction = new Dance();
                        break;
                    case Action.Color:
                        newAction = new Color();
                        break;
                    default:
                        throw new Exception("Fatal error.");
                }

                actionText.gameObject.SetActive(true);

                newAction.Activate();
                latestAction = newAction;

                timePassed = 0.0f;
                
                if(currentSpeed > MinSpeed)
                {
                    currentSpeed*=(1-SpeedChangePercentage/100);
                }

                if(DebugMode)
                {
                    //Debug.Log($"Current speed: {currentSpeed}");
                }
                //actionsStack.Enqueue(newAction);
            }
            else
            {
                if(latestAction != null &&
                   !isLastActionPressed && 
                   latestAction.CheckInput())
                {
                    actionText.gameObject.SetActive(false);
                    successText.gameObject.SetActive(true);
                    isLastActionPressed = true;
                    IncrementScore();
                }
            }
        }
    }

    public void UpdateActionText(string text)
    {
        actionText.text = "ACTION: " + text;
    }

    public void IncrementScore()
    {
        scoreText.text = "SCORE: " + ++currentScore;
    }

    public void onClick()
    {
        gameStarted = true;
        gameOver = false;
        scoreText.text = "SCORE: 0";
        actionText.text = "GET READY!";
        healthBarScale = 50.0f;
        healthBar.transform.localScale = new Vector3(healthBarScale, healthBar.transform.localScale.y);
        
        timePassed = 0;
        startButton.gameObject.SetActive(false);
    }
}
