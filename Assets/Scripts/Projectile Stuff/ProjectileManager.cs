using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] public GameObject[] projectile;
    private int randomResult;
    [SerializeField] [Tooltip("The speed the projectile launcher rotates around the player (Not visible in game).")] private float orbitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, orbitSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Launches a projectile that is sent towards the player.
    /// </summary>
    /// <param name="projectileLauncherPosition">
    /// The transform.position of the projectile launcher.
    /// </param>
    public void LaunchProjectile(GameObject projectileLauncher)
    {
        randomResult = Random.Range(1, projectile.Length);
        Instantiate(projectile[randomResult], projectileLauncher.transform.position, Quaternion.identity);
    }

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
    }
}
