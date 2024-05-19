using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Timer passiveScoreTimer;
    [SerializeField] private int totalSeconds = 0;
    [SerializeField] private int timerInterval = 1000;

    private bool isPaused;

    private GameObject playerobj;
    public GameObject deathEffect;
    public GameObject deathScreen;
    public Button restartButton;

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
        deathScreen.SetActive(false);
    }
    private void Update()
    {
        if (!playerobj.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            restartButton.onClick.Invoke();
        }
    }

    public void PlayerDead()
    {
        isPaused = true;
        Instantiate(deathEffect, playerobj.transform.position, Quaternion.Euler(90, 0, 0));
        playerobj.SetActive(false);
        ProjectileManager projectileManager = projectileThing.GetComponent<ProjectileManager>();
        projectileManager.DestroyAllProjectiles();
        projectileThing.SetActive(false);
        deathScreen.SetActive(true);
    }
    public void RestartGame()
    {
        deathScreen.SetActive(false);
        playerobj.SetActive(true);
        projectileThing.SetActive(true);
    }
}
