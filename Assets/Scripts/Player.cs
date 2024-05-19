using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public GameObject shield;
    private bool releaseParryBool;
    public bool isParrying;

    public int shieldHP = 5;
    public GameObject glassbreak;

    [Header("Audio")]
    [SerializeField] private AudioClip parryActivateClip;
    [SerializeField] private AudioClip shieldShatterClip;
    private AudioSource shieldHitSource;
    private AudioSource parryActivateSource;
    [SerializeField] private AudioClip shieldHitSound;

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
        shieldHP = 5;

        parryActivateSource = null;
        shieldHitSource = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            releaseParryBool = false;
            shield.SetActive(true);
            StartCoroutine(ParryWindow(.1f));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            releaseParryBool = true;
            StartCoroutine(ParryWindow(.1f));
            shieldHP = 5;
        }
    }

    private IEnumerator ParryWindow(float duration)
    {
        //Parry is on
        isParrying = true;
        shield.GetComponent<SpriteRenderer>().color = Color.white;
        //parryActivateSource = AudioManager.instance.AddSFX(parryActivateClip, false, parryActivateSource);
        yield return new WaitForSeconds(duration);
        //Parry is off
        isParrying = false;
        shield.GetComponent<SpriteRenderer>().color = Color.blue;
        if (releaseParryBool) shield.SetActive(false);
        yield return null;
        yield break;
    }

    public void ShieldDamage(int damage)
    {
        shieldHP -= damage;
        var color = shield.GetComponent<SpriteRenderer>();
        var tempColor = color.color;
        tempColor.a -= .2f;
        color.color = tempColor;
        //shieldHitSource = AudioManager.instance.AddSFX(shieldHitSound, false, shieldHitSource);
        if(shieldHP == 1)
        {
            // make color flash to warn player its about to break
        }
        if (shieldHP <= 0)
        {
            shield.SetActive(false);
            AudioManager.instance.PlaySound(AudioManagerChannels.SFXChannel, shieldShatterClip, 1f);  
            Instantiate(glassbreak, shield.transform.position, Quaternion.Euler(0, 0, 0));
            Debug.Log("shieldbreak");
        }
        if (shieldHP !>= 1)
        {
            shieldHitSource = AudioManager.instance.AddSFX(shieldHitSound, false, shieldHitSource);
        }
    }
}
