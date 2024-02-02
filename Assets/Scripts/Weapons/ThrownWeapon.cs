using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
  public float throwPower;
  public Rigidbody2D theRB;
  public float rotateSpeed;
  // Start is called before the first frame update
  void Start()
  {
    theRB.velocity = new Vector2(Random.Range(-throwPower, throwPower), throwPower);
  }

  // Update is called once per frame
  void Update()
  {
    transform.rotation = Quaternion.Euler(0f,0f, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * 360f * Mathf.Sign(theRB.velocity.x)));
  }
}
