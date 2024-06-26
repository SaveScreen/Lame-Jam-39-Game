using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private ProjectileManager projectileManager;
    public float launchSpeed;
    private float startinglaunchSpeed;

    // Start is called before the first frame update
    void Start()
    {
        projectileManager = GetComponentInParent<ProjectileManager>();

        startinglaunchSpeed = launchSpeed;

        projectileManager.LaunchProjectile(gameObject);
    }

    public void SetLaunchSpeed(float speed)
    {
        launchSpeed = speed;
        startinglaunchSpeed = launchSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPaused == false)
        {
            launchSpeed -= Time.deltaTime;
            if (launchSpeed <= 0f)
            {
                projectileManager.LaunchProjectile(gameObject);
                launchSpeed = startinglaunchSpeed;
            }
        }
        
    }

    public void ResetLauncher()
    {
        launchSpeed = startinglaunchSpeed;
    }

    public void LaunchStart()
    {
        projectileManager.LaunchProjectile(gameObject);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
