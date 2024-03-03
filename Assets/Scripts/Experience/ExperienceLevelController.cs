using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{

  public static ExperienceLevelController instance;
  

  private void Awake()
  {
    instance = this; 
  }

  public int currentExperience;
  public ExpPickup pickup;
  public List<int> experienceLevels;
  public int currentLevel = 1, maxLevel = 100;
  public float levelUpExpAdd = 1.1f;
  public List<Weapon> weaponsToUpgrade;
  void Start()
  {
    while (experienceLevels.Count <= maxLevel)
    {
      experienceLevels.Add(Mathf.CeilToInt(experienceLevels[experienceLevels.Count - 1] + levelUpExpAdd));
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void GetExp(int amountToGet)
  {
    currentExperience += amountToGet;
    if (currentExperience >= experienceLevels[currentLevel])
    {
      LevelUp();
    }
    UIController.instance.UpdateExperience(currentExperience, experienceLevels[currentLevel], currentLevel);
    SFXManager.instance.PlayeSFXPitched(2);
  }

  public void SpawnExp(Vector3 spawnPosition, int expValue)
  {
    Instantiate(pickup,spawnPosition,Quaternion.identity).expValue = expValue;
  }
  void LevelUp()
  {
    if (!NNManager.instance.TurnOffMechanicsForNN)
    {
      currentExperience -= experienceLevels[currentLevel];
      currentLevel++;
      if (currentLevel > maxLevel)
      {
        currentLevel--;
      }

      UIController.instance.levelUpPanel.SetActive(true);
      Time.timeScale = 0f;
      //UIController.instance.levelUpSelectionButtons[0].UpdateButtonDisplay(PlayerController.instance.activeWeapon);
      weaponsToUpgrade.Clear();
      List<Weapon> avaliableWeapons = new List<Weapon>();
      avaliableWeapons.AddRange(PlayerController.instance.assignedWeapons);

      if (avaliableWeapons.Count > 0)
      {
        int selected = Random.Range(0, avaliableWeapons.Count);
        weaponsToUpgrade.Add(avaliableWeapons[selected]);
        avaliableWeapons.RemoveAt(selected);
      }
      if (PlayerController.instance.assignedWeapons.Count + PlayerController.instance.maxedWeapons.Count < PlayerController.instance.maxWeapons)
        avaliableWeapons.AddRange(PlayerController.instance.unassignedWeapons);

      for (int i = weaponsToUpgrade.Count; i < 3; i++)
      {
        if (avaliableWeapons.Count > 0)
        {
          int selected = Random.Range(0, avaliableWeapons.Count);
          weaponsToUpgrade.Add(avaliableWeapons[selected]);
          avaliableWeapons.RemoveAt(selected);
        }
      }
      for (int i = 0; i < weaponsToUpgrade.Count; i++)
      {
        UIController.instance.levelUpSelectionButtons[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
      }

      for (int i = 0; i < UIController.instance.levelUpSelectionButtons.Length; i++)
      {
        if (i < weaponsToUpgrade.Count)
        {
          UIController.instance.levelUpSelectionButtons[i].gameObject.SetActive(true);
        }
        else
        {
          UIController.instance.levelUpSelectionButtons[i].gameObject.SetActive(false);
        }
      }



      PlayerStatController.instance.UpdateDisplays();
    }
  }
}
