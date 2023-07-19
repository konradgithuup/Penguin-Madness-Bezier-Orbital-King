using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingListener : MonoBehaviour
{
    

    public static Vector3 penguSpeed;
    public static int /*Datentyp ändern falls nötig*/ penguHeight;

    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject menuButton;

    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText = GameObject.Find("EndText");
        menuButton = GameObject.Find("MenuButton");
        restartButton = GameObject.Find("RestartButton");
        gameOverText.SetActive(false);
        menuButton.SetActive(false);
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(penguSpeed.magnitude >= 11200 /* m/s */ || penguHeight >= 5){
            EndGame();
        }
        if(gameEnded){
            gameOverText.SetActive(true);
//            Debug.Log(text.transform.position);
            if(gameOverText.transform.localPosition.y <= 10f){
                menuButton.SetActive(true);
                restartButton.SetActive(true);
                Time.timeScale = 0f;
            }
        } 
    }

    public void EndGame(){
        gameEnded = true;
        Debugger.isPaused = true;       
    }
}
