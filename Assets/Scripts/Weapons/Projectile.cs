using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public float moveSpeed;
  private float lifeTime = 5f;
  private float lifeCount;

  private void Start()
  {
    lifeCount = lifeTime;
  } 

  // Update is called once per frame
  void Update()
  {

    lifeCount -= Time.deltaTime;
    if (lifeCount <= 0)
    {
      Destroy(gameObject);
      lifeCount = lifeTime;
    }


    transform.position += transform.up * moveSpeed * Time.deltaTime;
  }
}
