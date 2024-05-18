using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] [Tooltip("The speed the projectile launcher rotates around the player (Not visible in game).")] private float orbitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Instantiate(projectile, projectileLauncher.transform.position, Quaternion.identity);
    }
}
