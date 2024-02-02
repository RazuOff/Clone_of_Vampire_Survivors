 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public List<WeaponStats> stats = new List<WeaponStats>();
  public int weaponLevel;

  [HideInInspector]
  public bool statsUpdated;
  public Sprite icon;
  public void LevelUp()
  {
    if (weaponLevel < stats.Count - 1 )
    {
      weaponLevel++;
      statsUpdated = true;

      if (weaponLevel>= stats.Count - 1 )
      {
        PlayerController.instance.maxedWeapons.Add(this);
        PlayerController.instance.assignedWeapons.Remove(this);
      }
    }
  }  
}

[System.Serializable]
public class WeaponStats
{
  public float attackSpeed, damage, range, cooldown, amount, duration;
  public string upgrateText;
}
