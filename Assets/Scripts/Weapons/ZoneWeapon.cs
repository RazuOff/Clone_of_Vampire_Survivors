using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
  public EnemyDamager enemyDamager;
  private float spawnTime, spawnCounter;
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

    spawnCounter -= Time.deltaTime;
    if (spawnCounter <= 0)
    {
      spawnCounter = spawnTime;
      Instantiate(enemyDamager, enemyDamager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
      SFXManager.instance.PlayeSFXPitched(10);
    }
  }

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.lifeTime = stats[weaponLevel].duration;
    enemyDamager.timeBetweenDamage = 1f/stats[weaponLevel].speed;
    
    enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;
    spawnTime = stats[weaponLevel].cooldown;
    spawnCounter = 0;
  }
}
