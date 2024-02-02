using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
  public static PlayerStatController instance;

  private void Awake()
  {
    instance = this;
  }


  public List<PlayerStatValue> moveSpeed, health, pickupRange, maxWeapons;
  public int moveSpeedMaxLevel, healthMaxLevel, pickupRangeMaxLevel;

  public int moveSpeedLevel, healthLevel, pickupRangeLevel, maxWeaponsLevel;
  // Start is called before the first frame update
  void Start()
  {
    GenerateStats(moveSpeed, moveSpeedMaxLevel);
    GenerateStats(health, healthMaxLevel);
    GenerateStats(pickupRange, pickupRangeMaxLevel);

  }

  // Update is called once per frame
  void Update()
  {
    if (UIController.instance.levelUpPanel.activeSelf)
    {
      UpdateDisplays();
    }
  }

  private void GenerateStats(List<PlayerStatValue> stat, int maxLevel)
  {
    for (int i = stat.Count - 1; i < maxLevel; i++)
    {
      stat.Add(new PlayerStatValue(stat[i].cost + stat[1].cost, stat[i].value + (stat[1].value - stat[0].value)));
    }
  }
  public void UpdateDisplays()
  {

    if (moveSpeedLevel < moveSpeed.Count - 1)
    {
      UIController.instance.moveSpeedUpgradeDisplay.UpdateDisplay(moveSpeed[moveSpeedLevel + 1].cost, moveSpeed[moveSpeedLevel].value, moveSpeed[moveSpeedLevel + 1].value);
    }
    else
      UIController.instance.moveSpeedUpgradeDisplay.ShowMaxLevel();
    if (healthLevel < health.Count - 1)
    {
      UIController.instance.healthUpgradeDisplay.UpdateDisplay(health[healthLevel + 1].cost, health[healthLevel].value, health[healthLevel + 1].value);
    }
    else
      UIController.instance.healthUpgradeDisplay.ShowMaxLevel();
    if (pickupRangeLevel < pickupRange.Count - 1)
    {

      UIController.instance.pickupRangeUpgradeDisplay.UpdateDisplay(pickupRange[pickupRangeLevel + 1].cost, pickupRange[pickupRangeLevel].value, pickupRange[pickupRangeLevel + 1].value);
    }
    else
      UIController.instance.pickupRangeUpgradeDisplay.ShowMaxLevel();
    if (maxWeaponsLevel < maxWeapons.Count - 1)
    {
      UIController.instance.maxWeaponsUpgradeDisplay.UpdateDisplay(maxWeapons[maxWeaponsLevel + 1].cost, maxWeapons[maxWeaponsLevel].value, maxWeapons[maxWeaponsLevel + 1].value);
    }
    else
      UIController.instance.maxWeaponsUpgradeDisplay.ShowMaxLevel();
  }

  public void BuyMoveSpeed()
  {
    moveSpeedLevel++;
    CoinController.instance.SpendCoins(moveSpeed[moveSpeedLevel].cost);
    UpdateDisplays();

    PlayerController.instance.moveSpeed = moveSpeed[moveSpeedLevel].value;
  }
  public void BuyHealth()
  {
    healthLevel++;
    CoinController.instance.SpendCoins(health[healthLevel].cost);
    UpdateDisplays();

    PlayerHealthController.instance.maxHealth = health[healthLevel].value;
    PlayerHealthController.instance.currentHealth += health[healthLevel].value - health[healthLevel - 1].value;
  }
  public void BuyPickUpRange()
  {
    pickupRangeLevel++;
    CoinController.instance.SpendCoins(pickupRange[pickupRangeLevel].cost);
    UpdateDisplays();

    PlayerController.instance.pickupRange = pickupRange[pickupRangeLevel].value;
  }
  public void BuyMaxWeapons()
  {
    maxWeaponsLevel++;
    CoinController.instance.SpendCoins(maxWeapons[maxWeaponsLevel].cost);
    UpdateDisplays();

    PlayerController.instance.maxWeapons = Mathf.RoundToInt(maxWeapons[maxWeaponsLevel].value);
  }

}

[System.Serializable]
public class PlayerStatValue
{
  public int cost;
  public float value;

  public PlayerStatValue(int cost, float value)
  {
    this.cost = cost;
    this.value = value;
  }
}
