using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume(){
        Debugger.isPaused = false;
        pauseMenuUI.SetActive(false);
        TimeScale();
        GameIsPaused = false;
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
        Debugger.isPaused = true;
        Time.timeScale = 1f;
    }


    async void TimeScale(){
        await Task.Delay(1000);
        Time.timeScale = 1f;
    }

    public void Pause(){
        Debugger.isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


}
