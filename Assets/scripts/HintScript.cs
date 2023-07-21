using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintScript : MonoBehaviour
{

    public GameObject hintUI;
    public static Text header;
    public Text content;

    public static bool clickHintShown = false;

    // Update is called once per frame
    void Update()
    {
        if (clickHintShown){
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                clickHint();
            }
        }
    }

    void clickHint()
    {
        hintUI.text = "Starting the journey";
        Text.text = "Press the left mouse button to place your first ice floe.";
        hintUI.SetActive(false);
    }
}
