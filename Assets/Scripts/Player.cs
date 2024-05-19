using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public GameObject shield;
    public GameObject shieldRadius;
    private bool releaseParryBool;
    public bool isParrying;

    public int shieldHP = 5;
    public GameObject glassbreak;

    private bool fullCharge;
    private Vector3 tempSize;
    private Vector3 shieldSize;
    private Vector3 tempRadius;
    private Vector3 radiusSize;
    private Coroutine lastRoutine = null;
    public ParticleSystem chargeEffect;
    public ParticleSystem superEffect;


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
        shieldSize = shield.transform.localScale;
        radiusSize = shieldRadius.transform.localScale;
        tempSize = shield.transform.localScale * 1.5f;
        tempRadius = shieldRadius.transform.localScale * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            releaseParryBool = false;
            shield.SetActive(true);
            StartCoroutine(ParryWindow(.1f));
            lastRoutine = StartCoroutine(ShieldCharge(1f));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(lastRoutine);
            chargeEffect.Stop();
            if (fullCharge)
            {
                shield.transform.localScale = tempSize;
                superEffect.Play();
                StartCoroutine(ParryWindow(.2f));
            }
            else
            {
                StartCoroutine(ParryWindow(.1f));
            }
            releaseParryBool = true;
            fullCharge = false;
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
        shield.GetComponent<SpriteRenderer>().color = Color.cyan;
        if (releaseParryBool) shield.SetActive(false);
        shield.transform.localScale = shieldSize;
        shieldRadius.transform.localScale = radiusSize;
        yield return null;
        yield break;
    }
    private IEnumerator ShieldCharge(float duration1)
    {
        Debug.Log("charge start");
        chargeEffect.Play();
        yield return new WaitForSeconds(duration1);
        fullCharge = true;
        Debug.Log("fully charged!");
        chargeEffect.Stop();
        shieldRadius.transform.localScale = tempRadius;
        yield return null;
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
            Instantiate(glassbreak, shield.transform.position, Quaternion.Euler(90, 0, 0));
            Debug.Log("shieldbreak");
        }
        if (shieldHP !>= 1)
        {
            shieldHitSource = AudioManager.instance.AddSFX(shieldHitSound, false, shieldHitSource);
        }
    }
}
