using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingListener : MonoBehaviour
{
    

    public static Vector3 penguSpeed;
    public static int /*Datentyp ändern falls nötig*/ penguHeight = 3;
    public static int endScore = 40000;

    public GameObject winText;
    public GameObject loseText;
    public GameObject restartButton;
    public GameObject menuButton;
    public TextMeshProUGUI scoreTextContent;

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
        scoreTextContent = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

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
            winText.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            loseText.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            scoreTextContent.text = "Score: " + endScore;
        }
    }

    public void WinGame(){
//        cameraSpeed = 0;
        gameWon = true;
    }

    public void LoseGame(){
//        cameraSpeed = 0;
        gameLost = true;
    }
}
