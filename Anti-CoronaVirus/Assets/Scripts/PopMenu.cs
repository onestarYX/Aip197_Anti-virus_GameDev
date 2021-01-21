using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool gameIsPaused = false;
}


public class PopMenu : MonoBehaviour
{
    public GameObject strategyMenu;
    public GameObject instrMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStrategyBtn()
    {
        strategyMenu.SetActive(true);
        Time.timeScale = 0f;
        Globals.gameIsPaused = true;
    }

    public void OnClickIntrBtn()
    {
        instrMenu.SetActive(true);
        Time.timeScale = 0f;
        Globals.gameIsPaused = true;
    }

    public void StrategyResume()
    {
        strategyMenu.SetActive(false);
        Time.timeScale = 1f;
        Globals.gameIsPaused = false;
    }

    public void InstrResume()
    {
        instrMenu.SetActive(false);
        Time.timeScale = 1f;
        Globals.gameIsPaused = false;
    }

    public void QuitProg()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
