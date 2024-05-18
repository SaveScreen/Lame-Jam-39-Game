using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstanceScript : MonoBehaviour
{
    private GameObject projectileOrigin;
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        projectileOrigin = GameObject.FindWithTag("ProjectileOrigin");
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 targetposition = Vector3.zero - transform.position;
        rb.velocity += speed * Time.deltaTime * targetposition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If this hits the shield
        //Ethan: if we want to differentiate between shield giving no score and parry giving score,
        //       shouldn't we use something other than CompareTag since that'd require us to change obj's tag in realtime?
        if (collision.gameObject.CompareTag("Shield"))
        {
            if(Player.Instance.isParrying)
            {
                //Bounce off the shield
                transform.SetParent(projectileOrigin.transform);
                speed = 0.25f;
                ScoreController.instance.AddScoreWithMultiplier(1);
            }
            else
            {
                Player.Instance.ShieldDamage(1);
            }

        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Remind me to add the death stuff");
            Destroy(gameObject);
            GameManager.instance.PlayerDead();
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
           Destroy(gameObject);
        }
    }
}