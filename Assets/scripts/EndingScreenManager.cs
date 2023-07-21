using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using gdg_playground.Assets.scripts;

public class EndingScreenManager : MonoBehaviour
{

    public TextMeshProUGUI endText;
    public GameObject restartButton;
    public GameObject menuButton;
    public TextMeshProUGUI scoreTextContent;

    public static bool gameWon = false;

    public static int endScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShopManager.points += endScore;
        endText = GameObject.Find("EndMessage").GetComponent<TextMeshProUGUI>();
        menuButton = GameObject.Find("MenuButton");
        restartButton = GameObject.Find("RestartButton");
        scoreTextContent = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); 

        if(gameWon){
            endText.text = "Congrats! You won :)";
        }     
        else {
            endText.text = "Game over!";
        }  
        scoreTextContent.text = "Score: " + endScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
