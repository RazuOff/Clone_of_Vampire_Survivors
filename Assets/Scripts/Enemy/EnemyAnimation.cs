using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
  public Transform sprite;

  public float speed;
  public float minSize, maxSize;
  private float activeSize;
  // Start is called before the first frame update
  void Start()
  {
    activeSize = maxSize;
    speed *= Random.Range(.75f, 1.25f);
  }

  // Update is called once per frame
  void Update()
  {
    sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed* Time.deltaTime);


    if (sprite.localScale.x == activeSize)
    {
      if (activeSize == maxSize)
        activeSize = minSize;
      else
        activeSize = maxSize;
    }

  }
}
