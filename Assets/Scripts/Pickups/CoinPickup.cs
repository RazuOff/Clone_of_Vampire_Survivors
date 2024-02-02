using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

  public int coinAmount = 1;
  private bool movingToPlayer;

  public float moveSpeed;
  public float timeBetweenCheck = .2f;
  private float checkCounter;

  private PlayerController player;
  // Start is called before the first frame update
  void Start()
  {
    player = PlayerController.instance;
  }

  // Update is called once per frame
  void Update()
  {
    if (movingToPlayer)
    {
      transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }
    else
    {
      checkCounter -= Time.deltaTime;
      if (checkCounter <= 0)
      {
        checkCounter = timeBetweenCheck;
        if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
        {
          movingToPlayer = true;
          moveSpeed += player.moveSpeed;
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Player")
    {
      CoinController.instance.addCoins(coinAmount);
      Destroy(gameObject);
    }
  }
}
