using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageNumberController : MonoBehaviour
{
  public static DamageNumberController instance;
  public DamageNumber numberToSpawn;
  public Transform numberCanvas;

  private List<DamageNumber> numberPool = new List<DamageNumber>();

  private void Awake()
  {
    instance = this;
  }




  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SpawnDamageNumber(float damage, Vector3 location)
  {
    int roundedDamage = Mathf.RoundToInt(damage);

    //DamageNumber newDamage = Instantiate(numberToSpawn, location, Quaternion.identity, numberCanvas);


    DamageNumber newDamage = GetFromPool();

    newDamage.Setup(roundedDamage);
    newDamage.gameObject.SetActive(true);

    newDamage.transform.position = location;
  }

  public DamageNumber GetFromPool()
  {
    DamageNumber numberToOutput = null;
    if (numberPool.Count == 0)
    {
      numberToOutput = Instantiate(numberToSpawn, numberCanvas);
    }
    else
    {
      numberToOutput = numberPool[0];
      numberPool.RemoveAt(0);
    }

    return numberToOutput;
  }

  public void PlaceInPool(DamageNumber numberToPlace)
  {
    numberToPlace.gameObject.SetActive(false);

    numberPool.Add(numberToPlace);
  }
}
