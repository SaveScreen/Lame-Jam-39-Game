using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Timer passiveScoreTimer;
    [SerializeField] private int totalSeconds = 0;
    [SerializeField] private int timerInterval = 1000;

    //[SerializeField] private int timerInterval1 = 2000;
    //private Timer passiveScoreTimer1;

    private bool isPaused;

    private GameObject playerobj;
    public GameObject deathEffect;
    public GameObject projectileThing; //this is temp till i bother to make a better way of removing projectiles + spawners on death
    public static GameManager instance { get; private set; }
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

        passiveScoreTimer = new Timer(timerInterval);
        passiveScoreTimer.Elapsed += HandleSecondTick;

       //passiveScoreTimer1 = new Timer(timerInterval1);
       //passiveScoreTimer1.Elapsed += HandleSecondTick1;
    }
    #endregion

    private void HandleSecondTick(object sender, ElapsedEventArgs e)
    {
        if(!isPaused)
        {
            totalSeconds += 1;
            ScoreController.instance.AddScore(1);

        }
        else
        {
            passiveScoreTimer.Stop();
            //passiveScoreTimer1.Stop();
        }
    }

    //Multiplier decay timer currently breaks the multiplier mechanic so im just gonna turn it off for now bcs its annoying
    /*
    private void HandleSecondTick1(object sender, ElapsedEventArgs e)
    {
        if (!isPaused)
        {
            ScoreController.instance.MultiplierDecay(.001f);
        }
        else
        {
            passiveScoreTimer1.Stop();
        }
    }
    */

    private void Start()
    {
        passiveScoreTimer.Start();
        //passiveScoreTimer1.Start();
        playerobj = FindObjectOfType<Player>().gameObject;
    }

    public void PlayerDead()
    {
        isPaused = true;
        Instantiate(deathEffect, playerobj.transform.position, Quaternion.Euler(90, 0, 0));
        playerobj.SetActive(false);
        projectileThing.SetActive(false);
    }
}
