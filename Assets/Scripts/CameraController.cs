using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Transform target;


  void Start()
  {
    target = FindAnyObjectByType<PlayerController>().transform;
  }

  // Update is called once per frame
  void LateUpdate()
  {
    transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
  }
}
