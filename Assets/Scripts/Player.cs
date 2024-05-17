using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    private IEnumerator coroutine;

    public static Player Instance { get { return _instance; } }
    public GameObject shield;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
// Start is called before the first frame update
void Start()
    {
        coroutine = ParryWindow(1f);
        shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ding!");
            
            shield.SetActive(true);
            StartCoroutine(coroutine);
            shield.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("dong!");
            shield.SetActive(false);
            StopCoroutine(coroutine);
        }
    }
    private IEnumerator ParryWindow(float duration)
    {
        Debug.Log("CanParry");
        shield.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(duration);
        Debug.Log("no more parry");
    }
}
