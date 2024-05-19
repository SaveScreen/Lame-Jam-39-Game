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
        }
    }

    private void Start()
    {
        passiveScoreTimer.Start();
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
