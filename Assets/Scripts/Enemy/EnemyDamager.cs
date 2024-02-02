using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
  public float damage = 5f;

  public float lifeTime, growSpeed = 5f;
  private Vector3 targetSize;
  public bool shouldKnockBack;
  public bool destroyParent;
  public bool damageOvertime;
  public float timeBetweenDamage;
  private float damageCounter;
  [HideInInspector]
  public bool projectile = false;
  public bool destroyOnImpact;
  public LayerMask whatIsEnemy;
  private List<EnemyController> enemiesInRange = new List<EnemyController>();
  void Start()
  {


    targetSize = transform.localScale;
    if (!projectile ) 
      transform.localScale = Vector3.zero;
  }

  // Update is called once per frame
  void Update()
  {
    if (!projectile)
    {
      transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

      lifeTime -= Time.deltaTime;
      if (lifeTime <= 0)
      {
        targetSize = Vector3.zero;

        if (transform.localScale.x == 0f)
        {
          Destroy(gameObject);

          if (destroyParent)
          {
            Destroy(transform.parent.gameObject);
          }
        }
      }
    }
    if (damageOvertime)
    {
      damageCounter -= Time.deltaTime;
      if (damageCounter <= 0)
      {
        damageCounter = timeBetweenDamage;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x + 0.2f, whatIsEnemy);
        //for (int i = 0; i < enemiesInRange.Count; i++)
        //{
        //  if (enemiesInRange[i] != null)
        //    enemiesInRange[i].TakeDamage(damage, shouldKnockBack);
        //  else
        //  {
        //    enemiesInRange.RemoveAt(i);
        //    i--;
        //  }

        //}

        for (int i = 0; i < enemies.Length; i++)
        {
          if (enemies[i] != null)
            enemies[i].gameObject.GetComponent<EnemyController>().TakeDamage(damage, shouldKnockBack);
        }
      }
    }
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if (!damageOvertime)
    {
      if (collision.tag == "Enemy")
      {
        collision.GetComponent<EnemyController>().TakeDamage(damage, shouldKnockBack);

        if (destroyOnImpact)
        {
          Destroy(gameObject);
        }
      }
    }
    else
    {
      if (collision.tag == "Enemy")
      {
        enemiesInRange.Add(collision.GetComponent<EnemyController>());        
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (damageOvertime)
    {
      if (collision.tag == "Enemy")
        enemiesInRange.Remove(collision.GetComponent<EnemyController>());
    }
  }
}
