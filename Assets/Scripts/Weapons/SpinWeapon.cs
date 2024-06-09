using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SpinWeapon : Weapon
{
  public float rotateSpeed;

  public Transform holder, fireballToSpawn;

  public float timeBettweenSpawn;
  private float spawnCounter;

  public EnemyDamager enemyDamager;
  void Start()
  {
    SetStats();

    //UIController.instance.levelUpSelectionButtons[0].UpdateButtonDisplay
  }

  // Update is called once per frame
  void Update()
  {
    //holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));


    spawnCounter -= Time.deltaTime;
    if (spawnCounter <= 0)
    {
      spawnCounter = timeBettweenSpawn;
      for (int i = 0; i < stats[weaponLevel].amount; i++)
      {
        float rotation = (360f / stats[weaponLevel].amount) * i;
        Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0f, 0f, rotation), holder).gameObject.SetActive(true);
      }
      SFXManager.instance.PlaySFX(8);
    }

    if (statsUpdated)
    {
      statsUpdated = false;
      SetStats();
    }
  }

  public void SetStats()
  {
    enemyDamager.damage = stats[weaponLevel].damage;

    transform.localScale = Vector3.one * stats[weaponLevel].range;
    timeBettweenSpawn = stats[weaponLevel].cooldown;
    enemyDamager.duration = stats[weaponLevel].duration;
    rotateSpeed = stats[weaponLevel].attackSpeed;
    spawnCounter = 0f;
  }
}