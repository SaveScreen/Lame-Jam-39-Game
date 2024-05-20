using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public GameObject self;
    public Vector3 pointA;
    public Vector3 pointB;
    public bool isShaking;
    public float speed1 = 1;

    private void Start()
    {
        pointA = new Vector3(self.transform.position.x + 5, self.transform.position.y, self.transform.position.z);
        pointB = new Vector3(self.transform.position.x - 5, self.transform.position.y, self.transform.position.z);

    }
    private void Update()
    {
        if(isShaking)
        {
            self.transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time * speed1, 1.0f));
        }
    }
    public void Shakeify(float speed)
    {
        if(speed < 1)
        {
            speed = 0;
        }
        speed1 = speed1 + speed;
        isShaking = true;
    }
}

