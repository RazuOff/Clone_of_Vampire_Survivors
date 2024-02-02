using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloseAttackWeapon : Weapon
{

  public EnemyDamager enemyDamager;
  private float attackCounter, direction;
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

    attackCounter -= Time.deltaTime;
    if (attackCounter <= 0 )
    {
      attackCounter = stats[weaponLevel].cooldown;
      direction = Input.GetAxisRaw("Horizontal");
      if (direction != 0)
      {
        if (direction > 0)
        {
          enemyDamager.transform.rotation = Quaternion.identity;
        }
        else
        {
          enemyDamager.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }        
      }
      
      Instantiate(enemyDamager, enemyDamager.transform.position, enemyDamager.transform.rotation, transform).gameObject.SetActive(true);

      for (int i = 1; i < stats[weaponLevel].amount; i++)
      {        
        float rotation = (360f / stats[weaponLevel].amount) * i;
        Instantiate(enemyDamager, enemyDamager.transform.position, Quaternion.Euler(0f, 0f, enemyDamager.transform.rotation.eulerAngles.z + rotation), transform).gameObject.SetActive(true);
      }
      SFXManager.instance.PlayeSFXPitched(9);
    }
  }

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.lifeTime = stats[weaponLevel].duration;

    enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;

    attackCounter = 0f;

    
  }
}
