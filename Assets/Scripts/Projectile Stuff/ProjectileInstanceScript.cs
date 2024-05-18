using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstanceScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 destination;
    private bool isHoming;
    private bool wasReflected;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 targetposition = Vector3.zero - transform.position;
        isHoming = true;
        wasReflected = false;
        rb.AddForce(targetposition * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoming)
        {
            
        }
        else 
        {
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If this hits the shield
        //Ethan: if we want to differentiate between shield giving no score and parry giving score,
        //       shouldn't we use something other than CompareTag since that'd require us to change obj's tag in realtime?
        if (collision.gameObject.CompareTag("Shield"))
        {
            //Bounce off the shield
            isHoming = false;
            ScoreController.instance.AddScoreWithMultiplier(1);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Remind me to add the death stuff");
            Destroy(gameObject);
            GameManager.instance.PlayerDead();
        }
    }
}
