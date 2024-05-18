using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private float currentScore;
    private float highScore;
    public float scoreValue;

    [SerializeField] private int multiplierValue; //current multiplier
    // i want multiplier value to be a float so we can make it a 1.2x multiplier for example, but it is not cooperating with the line 73 if statement unless its an int
    public float multiplierTracker;
    public float[] multiplierThresholds;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI multiplierText;
    public static ScoreController instance { get; private set; }

    // Singleton mode activated
    #region //singleton region
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        multiplierValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentScore > highScore)
        {
            highScore = currentScore;
        }
        scoreText.text = "Score: " + Mathf.Round(currentScore);
        highScoreText.text = "Hi-Score: " + Mathf.Round(highScore);
        multiplierText.text = "x" + multiplierValue;
        // Need to change these to use stringbuilder instead (learned it in opt and havent gotten to use it yet)


    }

    // function can be called from anywhere w/o direct reference bcs its a singleton
    // using Value instead of a preset value bcs we might want diff types of things to grant different amounts of score + multiplier will alter the amount of score recieved
    public void AddScore(float Value)
    {
        currentScore = currentScore + Value;
    }
    public void AddScoreWithMultiplier(float Value)
    {
        currentScore = currentScore + Mathf.Round(Value * multiplierValue);

        if (multiplierValue - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if(multiplierThresholds[multiplierValue - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                multiplierValue++;
            }
        }
    }
    public void MultiplierReset()
    {
        multiplierValue = 1;
        multiplierTracker = 0;
    }
}
