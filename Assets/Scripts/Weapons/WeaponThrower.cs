using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrower : Weapon
{
  public EnemyDamager enemyDamager;
  private float throwCounter;
  // Start is called before the first frame update
  void Start()
  {
    SetStats();
  }

  // Update is called once per frame
  void Update()
  {
    if (statsUpdated)
    {
      statsUpdated = false;
      SetStats();
    }

    throwCounter-=Time.deltaTime;
    if (throwCounter <= 0)
    {
      throwCounter = stats[weaponLevel].cooldown;
      for (int i = 0; i < stats[weaponLevel].amount; i++)
      {
        Instantiate(enemyDamager,enemyDamager.transform.position, enemyDamager.transform.rotation).gameObject.SetActive(true); 
      }
      SFXManager.instance.PlayeSFXPitched(4);
    }
  }

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.lifeTime = stats[weaponLevel].duration;
    

    enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;
    
    throwCounter = 0;
  }
}
