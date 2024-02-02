using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthController : MonoBehaviour
{
  public float currentHealth, maxHealth;
  public static PlayerHealthController instance;
  public Slider healthSlider;
  
  private void Awake()
  {
    instance = this;
  }
  public GameObject deathParicle;


  void Start()
  {

    maxHealth = PlayerStatController.instance.health[0].value;
    currentHealth = maxHealth;
    healthSlider.maxValue = maxHealth;
    healthSlider.value = currentHealth;
  }

  // Update is called once per frame
  void Update()
  {
    
  }


  public void TakeDamage(float damageToTake)
  {
    currentHealth -= damageToTake;

    if (currentHealth <=0)
    {
      gameObject.SetActive(false);
      LevelManager.instance.EndLevel();
      Instantiate(deathParicle, transform.position, transform.rotation);
      SFXManager.instance.PlaySFX(3);
    }
    healthSlider.value = currentHealth;
  }
}
