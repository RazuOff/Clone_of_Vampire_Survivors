using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollector : MonoBehaviour
{
  private Transform targetToFollow;
  public GameObject collector;

  private void Start()
  {
    targetToFollow = PlayerHealthController.instance.transform;
  }

  private void Update()
  {
    collector.transform.position = targetToFollow.position;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      Destroy(collision.gameObject);
    }

  }
}
