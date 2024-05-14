using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelUpSelectionButton : MonoBehaviour
{
  public TMP_Text upgradeDescText, nameLevelText;
  public Image weaponIcon;
  private Weapon assignedWeapon;
  // Start is called before the first frame update
  public void UpdateButtonDisplay(Weapon weapon)
  {
    if (weapon.gameObject.activeSelf)
    {


      upgradeDescText.text = weapon.stats[weapon.weaponLevel].upgrateText;
      weaponIcon.sprite = weapon.icon;
      nameLevelText.text = weapon.name + "- LVL " + weapon.weaponLevel;
    }
    else
    {
      upgradeDescText.text = "Unlock "+ weapon.name;
      weaponIcon.sprite = weapon.icon;
      nameLevelText.text = weapon.name;
    }
    assignedWeapon = weapon;
  }

  public void SelectUpgrade()
  {
    if (assignedWeapon != null)
    {
      if (assignedWeapon.gameObject.activeSelf)
      {
        assignedWeapon.LevelUp();
        
      }
      else
      {
        PlayerController.instance.AddWeapon(assignedWeapon);
      }
      UIController.instance.levelUpPanel.SetActive(false);
      Time.timeScale = UIController.instance.defultTimeScale;
    }
  }
}
