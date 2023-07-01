using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
  public int gameStartScene;

  public void LoadScene(){
    SceneManager.LoadScene(gameStartScene);
  }
}
