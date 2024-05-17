using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public GameObject shield;
    private bool releaseParryBool;

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
        shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ding!");
            releaseParryBool = false;
            shield.SetActive(true);
            StartCoroutine(ParryWindow(.1f));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("dong!");
            releaseParryBool = true;
            StartCoroutine(ParryWindow(.1f));
        }
    }

    private IEnumerator ParryWindow(float duration)
    {
        Debug.Log("CanParry");
        shield.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(duration);
        Debug.Log("no more parry");
        shield.GetComponent<SpriteRenderer>().color = Color.blue;
        if (releaseParryBool) shield.SetActive(false);
        yield return null;
        yield break;
    }

    public void Parry()
    {

    }
    public void ReleaseParry()
    {

    }
}
