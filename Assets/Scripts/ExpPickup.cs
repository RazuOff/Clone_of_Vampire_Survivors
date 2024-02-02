using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExpPickup : MonoBehaviour
{

  public int expValue;
  private bool movingToPlayer;

  public float moveSpeed;
  public float timeBetweenCheck = .2f;
  private float checkCounter;

  private PlayerController player;
  // Start is called before the first frame update
  void Start()
  {
    player = PlayerHealthController.instance.gameObject.GetComponent<PlayerController>();
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
      ExperienceLevelController.instance.GetExp(expValue);
      Destroy(gameObject);
    }
  }
}
