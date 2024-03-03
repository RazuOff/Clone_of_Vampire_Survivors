using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyNNController : EnemyController
{

  public float fitness { get; private set; }
  private NeuralNetwork brain;
  private float lifeTime = 30f, lifeTimer;
  //ќни не мен€ют решени€ во времени, а только при спавне
  public void SetBrain(NeuralNetwork brain)
  {
    this.brain = brain;
    fitness = 0;
    lifeTimer = lifeTime;
  }

  protected override void MoveToPlayer()
  {
    
    float[] inputs = new float[4];

    inputs[0] = targetTransfrom.position.x;
    inputs[1] = targetTransfrom.position.y;
    inputs[2] = transform.position.x;
    inputs[3] = transform.position.y;
    var output = brain.FeedForward(inputs);

    theRB.velocity = (new Vector3(output[0], output[1], transform.position.z)).normalized * moveSpeed;

    float frameFitness = (Mathf.Pow(dealtDamage, 2) + 1 / Vector3.Distance(transform.position, targetTransfrom.position));
    //if (frameFitness > fitness)
    //{
    //  fitness = frameFitness;
    //  brain.fitness = fitness;
    //}
    if (theRB.velocity == Vector2.zero)
    {
      frameFitness = 0;
    }
    fitness = frameFitness;
    brain.fitness = fitness;

    if (Vector3.Distance(transform.position, targetTransfrom.position) > 30f)
      this.TakeDamage(9999f);
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= 0)
    {
      this.TakeDamage(9999f);
    }
  }

  public override void TakeDamage(float damage)
  {
    health -= damage;
    if (health <= 0f)
    {

      NNManager.instance.brainsFromGeneration.Add(brain);
      Destroy(gameObject);
      //ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);

      //if (Random.value <= coinDropRate)
      //{
      //  CoinController.instance.DropCoin(transform.position, coinValue);
      //}

      SFXManager.instance.PlayeSFXPitched(0);
    }
    else
    {
      SFXManager.instance.PlayeSFXPitched(1);
    }

    DamageNumberController.instance.SpawnDamageNumber(damage, transform.position);
  }

}
