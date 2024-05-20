using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject howtoplay;
    private bool creditsOn;
    private bool howtoOn;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleCredits()
    {
        if (creditsOn == false)
        {
            mainmenu.SetActive(false);
            credits.SetActive(true);
            creditsOn = true;
        }
        else
        {
            mainmenu.SetActive(true);
            credits.SetActive(false);
            creditsOn = false;
        }
    }

    public void ToggleHowto()
    {
        if (howtoOn == false)
        {
            mainmenu.SetActive(false);
            howtoplay.SetActive(true);
            howtoOn = true;
        }
        else
        {
            mainmenu.SetActive(true);
            howtoplay.SetActive(false);
            howtoOn = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

}
