using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBoltWeapon : Weapon
{
  public EnemyDamager enemyDamager;
  public GameObject prepareZone;
  public float prepareTime;
  public Collider2D[] enemies;
  [SerializeField]
  private LayerMask whatIsEnemy;
  private float cooldown, cooldownTimer, prepareTimer, range;
  private bool findEnemy = false;
  private Collider2D enemy;
  private GameObject shot;
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
      
      if (!findEnemy)
      {
        enemies = Physics2D.OverlapCircleAll(transform.position, range, whatIsEnemy);
        if (enemies.Length != 0)
        {

          enemy = enemies[Random.Range(0, enemies.Length)];
          Vector3 targetPosition = enemy.transform.position;
          Vector3 direction = targetPosition - transform.position;
          //angle rotate to enemy
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          angle -= 90;
          transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

          prepareZone.SetActive(true);
          findEnemy = true;
        }
      }
      
      if (prepareZone.activeSelf & findEnemy)
        prepareTimer -= Time.deltaTime;
      if (prepareTimer <= 0)
      {

        prepareZone.SetActive(false);
        //Destroy(zone);
        shot = Instantiate(enemyDamager, enemyDamager.transform.position, enemyDamager.transform.rotation, transform).gameObject;
        shot.SetActive(true);
        findEnemy = false;
        prepareTimer = prepareTime;
        cooldownTimer = cooldown;
      }
    }
    if (shot != null)
    {
      prepareTimer = prepareTime;
      cooldownTimer = cooldown;
    }
  }

 

  void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;
    enemyDamager.duration = stats[weaponLevel].duration;
    enemyDamager.attackSpeed = stats[weaponLevel].attackSpeed;
    enemyDamager.transform.localScale = Vector3.one * stats[weaponLevel].range;
    range = enemyDamager.GetComponent<BoxCollider2D>().size.x;
    cooldown = stats[weaponLevel].cooldown;

    cooldownTimer = 0;
    prepareTimer = prepareTime;
  }
}
