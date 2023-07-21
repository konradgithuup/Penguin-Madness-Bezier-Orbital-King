using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingScreenManager : MonoBehaviour
{

    public TextMeshProUGUI endText;
    public GameObject restartButton;
    public GameObject menuButton;
    public TextMeshProUGUI scoreTextContent;

    public static bool gameWon = false;

    public int endScore;

    // Start is called before the first frame update
    void Start()
    {   
        endText = GameObject.Find("EndMessage").GetComponent<TextMeshProUGUI>();
        menuButton = GameObject.Find("MenuButton");
        restartButton = GameObject.Find("RestartButton");
        scoreTextContent = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); 

        if(gameWon){
            endText.text = "Congrats! You won!";
        }     
        else {
            endText.text = "Game over!";
        }  

        endScore = 4000;
        scoreTextContent.text = "Score: " + endScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
