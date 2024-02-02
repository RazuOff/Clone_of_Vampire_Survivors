using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

  public Rigidbody2D theRB;
  public float moveSpeed;
  private Transform targetTransfrom;

  public float damage;

  public float hitWaitTime = 1f;
  private float hitCounter;

  public float health = 5f;

  public float knockBackTime = .5f;
  private float knockBackCounter;

  public int expToGive = 1;
  public int coinValue = 1;
  public float coinDropRate = .5f;
  // Start is called before the first frame update
  void Start()
  {
    
    targetTransfrom = PlayerHealthController.instance.transform;

  }

  // Update is called once per frame
  void Update()
  {
    if (PlayerController.instance.gameObject.activeSelf)
    {
      if (knockBackCounter > 0)
      {
        knockBackCounter -= Time.deltaTime;

        if (moveSpeed > 0)
          moveSpeed = -moveSpeed * 2f;

        if (knockBackCounter <= 0)
          moveSpeed = Mathf.Abs(moveSpeed * .5f);
      }

      theRB.velocity = (targetTransfrom.position - transform.position).normalized * moveSpeed;

      if (hitCounter > 0f)
      {
        hitCounter -= Time.deltaTime;
      }
    }
    else
    {
      theRB.velocity = Vector2.zero;
    }
  }

  private void OnCollisionStay2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
    {
      PlayerHealthController.instance.TakeDamage(damage);
      hitCounter = hitWaitTime;
    }
  }

  public void TakeDamage(float damage)
  {
    health -= damage;
    if (health <= 0f)
    {
      Destroy(gameObject);
      ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);

      if (Random.value <= coinDropRate)
      {
        CoinController.instance.DropCoin(transform.position,coinValue);
      }

      SFXManager.instance.PlayeSFXPitched(0);
    }
    else
    {
      SFXManager.instance.PlayeSFXPitched(1);
    }

    DamageNumberController.instance.SpawnDamageNumber(damage, transform.position);
  }

  public void TakeDamage(float damage, bool shouldKnockBack)
  {
    TakeDamage(damage);

    if (shouldKnockBack)
    {
      knockBackCounter = knockBackTime;

    }
  }
  
}
