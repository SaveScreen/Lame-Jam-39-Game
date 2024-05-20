using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Timers;

public class ScoreController : MonoBehaviour
{
    private float currentScore;
    private float highScore;
    public float scoreValue;

    [SerializeField] private float multiplierValue; //current multiplier
    public float multiplierTracker;
    public float[] multiplierThresholds;

    public Slider multiplierBar;

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
        multiplierBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentScore > highScore)
        {
            highScore = currentScore;
        }
        if(multiplierValue < 1)
        {
            multiplierValue = 1;

        }
        

        
        scoreText.text = "Score: " + Mathf.Round(currentScore).ToString();
        highScoreText.text = "Hi-Score: " + Mathf.Round(highScore).ToString();
        multiplierText.text = "x" + multiplierValue.ToString();
        // Need to change these to use stringbuilder instead (learned it in opt and havent gotten to use it yet)

        //these are not working quite right. They kinda fill up the bar, but not as smoothly as I would like
        multiplierBar.value = multiplierTracker;
        if (multiplierValue < multiplierThresholds.Length)
        {
            multiplierBar.maxValue = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1];
        }
        else
        {
            multiplierBar.maxValue = multiplierThresholds[5];
        }
        


        if (multiplierValue > 1)
        {
            if (multiplierTracker < 0)
            {
                multiplierTracker = 0;
            }
            else
            {
                MultiplierDecay(.05f * multiplierValue);
            }

        }
        //this is meant to check if the multiplier tracker goes below 0 while the multiplier is above 1, decrease the multiplier level by 1 & make the multiplier bar just barely below its maximum. but its not working and wasting time rn



    }

    // function can be called from anywhere w/o direct reference bcs its a singleton
    // using Value instead of a preset value bcs we might want diff types of things to grant different amounts of score + multiplier will alter the amount of score recieved
    public void AddScore(float Value)
    {
        currentScore = currentScore + Value;
    }
    public void AddScoreWithMultiplier(float Value)
    {

        if (multiplierValue - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] <= multiplierTracker)
            {
                multiplierTracker = 1;
                Mathf.Round(multiplierValue++);
            }
        }
        if(multiplierValue - 1 > multiplierThresholds.Length)
        {
            Debug.Log("max multiplier reached");
            return;
        }

        currentScore = currentScore + Mathf.Round(Value * multiplierValue);

    }
    public void MultiplierReset()
    {
        multiplierValue = 1;
        multiplierTracker = 0;
    }
    public void MultiplierDecrease(int amt)
    {
        multiplierTracker -= amt;
        if (multiplierValue > 1 && multiplierTracker < 0)
        {
            multiplierValue = multiplierValue - 1;
            multiplierTracker = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] - .5f;
        }
    }
        

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void MultiplierDecay(float amtLost)
    {
        multiplierTracker -= amtLost * Time.deltaTime;
        if (multiplierValue > 1 && multiplierTracker < 0)
        {
            multiplierValue = multiplierValue - 1;
            multiplierTracker = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] - .5f;
        }
    }
}
