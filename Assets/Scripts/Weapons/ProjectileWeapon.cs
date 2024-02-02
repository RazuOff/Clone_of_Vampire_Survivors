using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
  public EnemyDamager enemyDamager;
  public Projectile projectile;
  private float shotCounter;
  private float range;
  public LayerMask whatIsEnemy;
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

    shotCounter -=Time.deltaTime;
    if (shotCounter <= 0)
    {
      shotCounter = stats[weaponLevel].cooldown;

      Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, whatIsEnemy);
      if (enemies.Length > 0)
      {
        for (int i = 0; i < stats[weaponLevel].amount; i++)
        {
          Vector3 targetPosition = enemies[Random.Range(0,enemies.Length)].transform.position;
          Vector3 direction = targetPosition - transform.position;
          //angle rotate to enemy
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          angle -= 90;
          projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

          Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
        }

        SFXManager.instance.PlayeSFXPitched(6);
      }

    }
  }

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.projectile = true;
    range = stats[weaponLevel].range;
    //enemyDamager.timeBetweenDamage = 1f / stats[weaponLevel].speed;

    //enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;
    
    shotCounter = 0;

    projectile.moveSpeed = stats[weaponLevel].speed;
  }
}
