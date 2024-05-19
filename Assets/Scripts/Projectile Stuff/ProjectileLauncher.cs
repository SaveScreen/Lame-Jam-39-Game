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

    // Update is called once per frame
    void Update()
    {
        launchSpeed -= Time.deltaTime;
        if (launchSpeed <= 0f)
        {
            projectileManager.LaunchProjectile(gameObject);
            launchSpeed = startinglaunchSpeed;
        }
    }
}
