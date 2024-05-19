using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectile;

    [Header("LAUNCHER PREFAB ONLY!!")]
    [SerializeField] private GameObject projectileLauncher;
    private GameObject startingProjectileLauncher;
    private Vector2 projectileLauncherStartingPosition;
    private int randomResult;
    [Header("")]
    [SerializeField] [Tooltip("The speed the projectile launcher rotates around the player (Not visible in game).")] private float orbitSpeed;
    [SerializeField] [Tooltip("Amount of time between projectile launchers being spawned")] private float launcherSpawnTimer;
    private float startingLauncherSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        startingProjectileLauncher = transform.GetChild(0).gameObject; //By default, there should only be one projectile launcher on the manager.
        projectileLauncherStartingPosition = startingProjectileLauncher.transform.position;

        transform.eulerAngles = Vector3.zero;
        startingLauncherSpawnTimer = launcherSpawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPaused == false)
        {
            transform.Rotate(Vector3.back, orbitSpeed * Time.deltaTime);

            launcherSpawnTimer -= Time.deltaTime;
            if (launcherSpawnTimer <= 0f)
            {
                Instantiate(projectileLauncher, projectileLauncherStartingPosition, Quaternion.identity, gameObject.transform);
                launcherSpawnTimer = startingLauncherSpawnTimer;
            }
        }  
    }

    public void LaunchOnNewGame()
    {
        ProjectileLauncher plauncher = startingProjectileLauncher.GetComponent<ProjectileLauncher>();
        plauncher.LaunchStart();
    }

    /// <summary>
    /// Launches a projectile that is sent towards the player.
    /// </summary>
    /// <param name="projectileLauncherPosition">
    /// The transform.position of the projectile launcher.
    /// </param>
    public void LaunchProjectile(GameObject projectileLauncher)
    {
        randomResult = Random.Range(1, projectile.Length+1);
        Instantiate(projectile[randomResult], projectileLauncher.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Destroys all projectiles on screen and resets the manager.
    /// </summary>
    public void DestroyAllProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        
        foreach (GameObject projectile in projectiles)
        {
            ProjectileInstanceScript pis = projectile.GetComponent<ProjectileInstanceScript>();
            pis.DestroySelf();
        }

        ResetManager();
    }

    void ResetManager()
    {
        transform.eulerAngles = Vector3.zero;
        launcherSpawnTimer = startingLauncherSpawnTimer;

        //Destroys all additional launchers except for the first one.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i > 0)
            {
                GameObject plauncher = transform.GetChild(i).gameObject;
                ProjectileLauncher plauncherScript = plauncher.GetComponent<ProjectileLauncher>();
                plauncherScript.DestroySelf();
            }
            else
            {
                startingProjectileLauncher.transform.position = projectileLauncherStartingPosition;
                ProjectileLauncher plauncherScript = startingProjectileLauncher.GetComponent<ProjectileLauncher>();
                plauncherScript.ResetLauncher();
            }
        }
    }
}
