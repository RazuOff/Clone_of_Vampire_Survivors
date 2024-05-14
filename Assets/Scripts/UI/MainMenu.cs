using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

  public string firstLevelName;
  public string secondLevelName;

  public void StartGame()
  {
    SceneManager.LoadScene(firstLevelName);
  }
  public void LearnNN()
  {
    SceneManager.LoadScene(secondLevelName);
  }

  public void QuitGame()
  {
    Application.Quit();

    Debug.Log("Quit");
  }
}
