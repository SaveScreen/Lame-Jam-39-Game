using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private AudioManager audioManager;
    private Timer passiveScoreTimer;
    [SerializeField] private int totalSeconds = 0;
    [SerializeField] private int timerInterval = 1000;

    [HideInInspector] public bool isPaused;
    private bool forcedDelay;

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
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
        passiveScoreTimer.Start();
        playerobj = FindObjectOfType<Player>().gameObject;
        deathScreen.SetActive(false);
    }
    private void Update()
    {
        if (!playerobj.activeSelf && Input.GetKeyDown(KeyCode.Space) && !forcedDelay)
        {
            restartButton.onClick.Invoke();
        }
    }

    public void PlayerDead()
    {
        isPaused = true;
        StartCoroutine(RestartAfterDelay());
        Instantiate(deathEffect, playerobj.transform.position, Quaternion.Euler(90, 0, 0));
        playerobj.SetActive(false);
        ProjectileManager projectileManager = projectileThing.GetComponent<ProjectileManager>();
        projectileManager.DestroyAllProjectiles();
        projectileThing.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void RestartGame()
    {
        isPaused = false;
        passiveScoreTimer.Start();
        ScoreController.instance.MultiplierReset();
        ScoreController.instance.ResetScore();
        deathScreen.SetActive(false);
        playerobj.SetActive(true);
        projectileThing.SetActive(true);
        ProjectileManager projectileManager = projectileThing.GetComponent<ProjectileManager>();
        projectileManager.LaunchOnNewGame();
        AudioManager.instance.PlayMusic("SampleScene");
    }

    IEnumerator RestartAfterDelay()
    {
        forcedDelay = true;
        yield return new WaitForSeconds(1f);
        forcedDelay = false;
    }
}
