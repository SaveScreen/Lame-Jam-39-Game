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
    private float prevmult;
    public float multiplierTracker;
    public float[] multiplierThresholds;

    public Slider multiplierBar;
    private Shake textShaker;
    public ParticleSystem fire;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI multiplierText;
    public Color defaultTextColor;
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
        prevmult = multiplierValue;
        textShaker = multiplierText.GetComponent<Shake>();
        defaultTextColor = scoreText.color;
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


        // Need to change these to use stringbuilder instead (learned it in opt and havent gotten to use it yet)

        //these are not working quite right. They kinda fill up the bar, but not as smoothly as I would like
        multiplierBar.value = multiplierTracker;
        if (multiplierValue < multiplierThresholds.Length)
        {
            multiplierBar.maxValue = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1];
        }
        else
        {
            //prevent maximum from exceeding the multiplier array range
            multiplierBar.maxValue = multiplierThresholds[5];
        }
        

        if (multiplierValue > 1)
        {
            //only decay the multiplier bar if the multiplier is above 1
            if (multiplierTracker < 0)
            {
                multiplierTracker = 0;
            }
            else
            {
                MultiplierDecay(.05f * multiplierValue);
            }
        }
        //check if multiplier is at max value and give a special text indicator if it is (this is hard to reach lol)
        if (multiplierValue < 7)
        {
            multiplierText.text = "x" + multiplierValue.ToString();
        }
        else if (multiplierValue >= 7)
        {
            multiplierText.text = "MAX x" + multiplierValue.ToString();
        }

        if (prevmult < multiplierValue)
        {
            //multiplier has increased
            //increase shake speed and text size
            multiplierText.transform.localScale = multiplierText.transform.localScale * 1.2f;
            prevmult = multiplierValue;
            textShaker.Shakeify(1.2f * multiplierValue);
        }
        else if (prevmult > multiplierValue)
        {
            //multiplier has decreased
            if(multiplierValue > 1)
            {
                //decrease shake speed and text size by same amount as the increase, but inverted
                multiplierText.transform.localScale = multiplierText.transform.localScale * .833f;
                textShaker.Shakeify(.833f * multiplierValue);
            }
            else if (multiplierValue <= 1)
            {
                // if multiplier is 1, force speed 0 and size back to original
                textShaker.speed1 = 0;
                multiplierText.transform.localScale = new Vector3(1, 1, 1);
            }
            prevmult = multiplierValue;
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
            //if multiplier has not reached max, increase the tracker by 1
            multiplierTracker++;
            if (multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] <= multiplierTracker)
            {
                //if the tracker has exceeded the required number to upgrade, increase multiplier and set the tracker back down to not quite 0 to prevent the auto decay from instakilling ur multiplier
                multiplierTracker = 1;
                Mathf.Round(multiplierValue++);
                if (multiplierValue >= 5)
                {
                    fire.Play();
                    multiplierText.GetComponent<TextMeshProUGUI>().color = Color.red;
                }
            }
        }
        if(multiplierValue - 1 > multiplierThresholds.Length)
        {
            Debug.Log("max multiplier reached");
            return;
        }
        //I dont think this math adds up right, but apply multiplier to score
        currentScore = currentScore + Mathf.Round(Value * multiplierValue);

    }
    public void MultiplierReset()
    {
        //reset the multiplier all the way
        multiplierValue = 1;
        multiplierTracker = 0;
        fire.Stop();
        multiplierText.GetComponent<TextMeshProUGUI>().color = defaultTextColor;
    }
    public void MultiplierDecrease(int amt)
    {
        //decrease multiplier by certain amout
        multiplierTracker -= amt;
        if (multiplierValue > 1 && multiplierTracker < 0)
        {
            multiplierValue = multiplierValue - 1;
            multiplierTracker = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] - .5f;
            fire.Stop();
            multiplierText.GetComponent<TextMeshProUGUI>().color = defaultTextColor;
        }

    }
        

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void MultiplierDecay(float amtLost)
    {
        //decrease multiplier by amount, but this one is used specifically for the over time decrease
        //probably not best that these are seperate, but it works
        multiplierTracker -= amtLost * Time.deltaTime;
        if (multiplierValue > 1 && multiplierTracker < 0)
        {
            multiplierValue = multiplierValue - 1;
            multiplierTracker = multiplierThresholds[(int)Mathf.Round(multiplierValue) - 1] - .5f;
            fire.Stop();
            multiplierText.GetComponent<TextMeshProUGUI>().color = defaultTextColor;
        }
    }
}
