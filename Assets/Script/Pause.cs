using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject Pausemenu;
    public GameObject howToPlayCanvas;
    public string sceneName;
    public bool toggle;
    public SC_FPSController playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript.enabled = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggle = !toggle;
            if(toggle == false)
            {
                Pausemenu.SetActive(false);
                Time.timeScale = 1;
                playerScript.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if(toggle == true)
            {
                Pausemenu.SetActive(true);
                Time.timeScale = 0;
                playerScript.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    public void resumeGame()
    {
        toggle = false;
        Pausemenu.SetActive(false);
        Time.timeScale = 1;
        playerScript.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Mainmenu()
    {
        SceneManager.LoadScene(sceneName);
        playerScript.enabled = true;
        Time.timeScale = 1;
    }
    public void Exit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ShowHowToPlay()
    {
        howToPlayCanvas.SetActive(true);
        Pausemenu.SetActive(false);
    }

    public void HideHowToPlay()
    {
        howToPlayCanvas.SetActive(false);
        Pausemenu.SetActive(true);
    }
}
