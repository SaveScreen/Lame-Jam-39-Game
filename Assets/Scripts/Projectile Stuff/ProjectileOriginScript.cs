using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOriginScript : MonoBehaviour
{
    [Tooltip("The speed the projectiles orbit around the player if they are parried")] public float orbitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, orbitSpeed * Time.deltaTime);
    }
}
