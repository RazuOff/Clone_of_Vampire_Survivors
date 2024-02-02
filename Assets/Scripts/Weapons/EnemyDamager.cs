using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
  [HideInInspector]
  public float duration;
  [HideInInspector]
  public float attackSpeed;
  [HideInInspector]
  public float damage = 5f;

  [HideInInspector]
  public bool projectile = false;

  [SerializeField]
  private float growSpeed = 5f;
  [SerializeField]
  private bool shouldKnockBack, destroyParent, isZone, destroyOnImpact;
  [SerializeField]
  private LayerMask whatIsEnemy;
  private float timeBetweenDamageTimer;
  private Vector3 targetSize;
  private float timeBetweenDamage;
  private bool firstAttackWithoutDelay = true;

  void Start()
  {
    timeBetweenDamage = 1 / attackSpeed;
    LifetimeRealizationSetup();
  }
  
  void Update()
  {

    LifeTimeRealization();

    if (isZone)
    {
      ZoneDamage();
    }
  }


  private void LifetimeRealizationSetup()
  {
    targetSize = transform.localScale;
    if (!projectile)
      transform.localScale = Vector3.zero;
  }
  private void ZoneDamage()
  {
    Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x + 0.2f, whatIsEnemy);
    if (enemies is not null && firstAttackWithoutDelay)
    {
      timeBetweenDamageTimer = 0;
      firstAttackWithoutDelay = false;
    }
    else
      timeBetweenDamageTimer -= Time.deltaTime;

    if (timeBetweenDamageTimer <= 0)
    {
      timeBetweenDamageTimer = timeBetweenDamage;
      for (int i = 0; i < enemies.Length; i++)
      {
        if (enemies[i] != null)
          enemies[i].gameObject.GetComponent<EnemyController>().TakeDamage(damage, shouldKnockBack);
      }
    }
    
  }

  private void LifeTimeRealization()
  {
    if (!projectile)
    {
      transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

      duration -= Time.deltaTime;
      if (duration <= 0)
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
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!isZone)
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
    
  }

  
}
