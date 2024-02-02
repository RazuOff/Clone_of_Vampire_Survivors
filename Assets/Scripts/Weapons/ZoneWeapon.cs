using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
  public EnemyDamager enemyDamager;
  private float cooldown, cooldownTimer;
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

    cooldownTimer -= Time.deltaTime;
    if (cooldownTimer <= 0)
    {
      cooldownTimer = cooldown;
      Instantiate(enemyDamager, enemyDamager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
      SFXManager.instance.PlayeSFXPitched(10);
    }
  }

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.duration = stats[weaponLevel].duration;
    enemyDamager.attackSpeed = stats[weaponLevel].attackSpeed;
    enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;
    cooldown = stats[weaponLevel].cooldown;
    cooldownTimer = 0;
  }
}
