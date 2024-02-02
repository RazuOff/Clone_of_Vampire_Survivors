using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

  public static UIController instance;

  private void Awake()
  {
    instance = this;
  }

  public Slider expLvlSlider;
  public TMP_Text expLvlText;
  public LevelUpSelectionButton[] levelUpSelectionButtons;
  public GameObject levelUpPanel;
  public TMP_Text coinText;
  public PlayerStatUpgradeDisplay moveSpeedUpgradeDisplay, healthUpgradeDisplay, pickupRangeUpgradeDisplay, maxWeaponsUpgradeDisplay;
  public TMP_Text timeText;
  public GameObject levelEndScreen;
  public TMP_Text endTimeText;
  public string MainMenuName;
  public GameObject pauseScreen;


  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      PauseUnpause();
    }
  }
  public void UpdateExperience(int currentExperience, int levelExp, int currentLevel)
  {
    expLvlSlider.maxValue = levelExp;
    expLvlSlider.value = currentExperience;

    expLvlText.text = "Level: " + currentLevel;
  }
  public void SkipLevelUp()
  {
    levelUpPanel.SetActive(false);
    Time.timeScale = 1.0f;
  }
  public void UpdateCoins()
  {
    coinText.text = ""+CoinController.instance.currentCoins;
  }
  public void UpdateTimer(float time)
  {
    float minutes =Mathf.FloorToInt(time / 60f);
    float seconds = Mathf.FloorToInt( time % 60);

    timeText.text = "Time: " + minutes + ":" + seconds.ToString("00");
  }

  public void BuyMoveSpeed()
  {
    PlayerStatController.instance.BuyMoveSpeed();
    SkipLevelUp();
  }
  public void BuyHealth()
  {
    PlayerStatController.instance.BuyHealth();
    SkipLevelUp();
  }
  public void BuyPickUpRange()
  {
    PlayerStatController.instance.BuyPickUpRange();
    SkipLevelUp();
  }
  public void BuyMaxWeapons()
  {
    PlayerStatController.instance.BuyMaxWeapons();
    SkipLevelUp();
  }

  public void GoToMainMenu()
  {
    SceneManager.LoadScene(MainMenuName);
    Time.timeScale = 1;
  }

  public void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    Time.timeScale = 1;
  }

  public void QuitGame()
  {
    Application.Quit();
  }
  public void PauseUnpause()
  {
    if (pauseScreen.activeSelf)
    {
      
      pauseScreen.SetActive(false);
      if (!levelUpPanel.activeSelf)
        Time.timeScale = 1;
    }
    else
    {
      pauseScreen.SetActive(true);
      Time.timeScale = 0;
    }
  }
}
