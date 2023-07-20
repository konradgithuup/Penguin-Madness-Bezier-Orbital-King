using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingListener : MonoBehaviour
{
    

    public static Vector3 penguSpeed;
    public static int /*Datentyp ändern falls nötig*/ penguHeight = 3;

    public GameObject winText;
    public GameObject loseText;
    public GameObject restartButton;
    public GameObject menuButton;

    private bool gameWon = false;
    private bool gameLost = false;

    private float endTextY = 5;

    // Start is called before the first frame update
    void Start()
    {
        winText = GameObject.Find("WinText");
        loseText = GameObject.Find("LoseText");
        menuButton = GameObject.Find("MenuButton");
        restartButton = GameObject.Find("RestartButton");
        winText.SetActive(false);
        loseText.SetActive(false);
        menuButton.SetActive(false);
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((penguSpeed.magnitude >= 11200 /* m/s */ || penguHeight >= 5) && !gameLost){
            WinGame();
        }
        else if(penguHeight <= 0 && !gameWon){
            LoseGame();
        }

        if(gameWon){
            winText.SetActive(true);
        } 
        else if(gameLost){
            loseText.SetActive(true);
        } 

        if(winText.transform.localPosition.y <= endTextY || loseText.transform.localPosition.y <= endTextY){
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void WinGame(){
        gameWon = true;
    }

    public void LoseGame(){
        gameLost = true;
    }
}
