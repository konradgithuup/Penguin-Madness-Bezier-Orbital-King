using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingListener : MonoBehaviour
{
    

    public Vector3 penguSpeed;
    public float penguHeight = int.MaxValue;
    public int endScore = 40000;
    public GameObject penguBody;

    // Start is called before the first frame update
    void Start()
    {
        penguBody = GameObject.Find("Body");
        
    }

    // Update is called once per frame
    void Update()
    {
        penguHeight = penguBody.transform.localPosition.y;
        //Win
        if(/*penguSpeed.magnitude >= 11200 /* m/s  &&*/ penguHeight >= 50){
            EndingScreenManager.gameWon = true;
            SceneManager.LoadScene(5);
        }
        //Lose
        else if(penguHeight <= -6){
            EndingScreenManager.gameWon = false;
            SceneManager.LoadScene(5);
        }
    }
}
