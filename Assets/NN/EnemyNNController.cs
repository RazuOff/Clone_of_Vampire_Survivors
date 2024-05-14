using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyNNController : EnemyController
{

  public float fitness { get; private set; }
  [SerializeField] private float visionRadius = 3f;
  [SerializeField] private LayerMask allyMask;
  private List<Vector2> allies = new List<Vector2>();
  private NeuralNetwork brain;
  private float lifeTime = 30f, lifeTimer;

  //ќни не мен€ют решени€ во времени, а только при спавне
  public void SetBrain(NeuralNetwork brain)
  {
    this.brain = brain;
    fitness = 0;
    lifeTimer = lifeTime;
  }


  private void FixedUpdate()
  {
    FindAllies();
  }
  protected override void MoveToPlayer()
  {

    float[] inputs = new float[NNManager.instance.countOfInputNeurons];

    inputs[0] = targetTransfrom.position.x;
    inputs[1] = targetTransfrom.position.y;
    inputs[2] = transform.position.x;
    inputs[3] = transform.position.y;
    List<Vector2> outputs = new List<Vector2>();
    outputs.AddRange(allies);
    
    int len = allies.Count;
    for (int i = 4; i < len + 4; i += 2)
    {
      if (i >= NNManager.instance.countOfInputNeurons)
        break;

      inputs[i] = outputs[0].x;
      inputs[i + 1] = outputs[0].y;
      outputs.RemoveAt(0);
    }


    var output = brain.FeedForward(inputs);

    theRB.velocity = (new Vector3(output[0], output[1], transform.position.z)).normalized * moveSpeed;

    //float frameFitness = (Mathf.Pow(dealtDamage, 2) + 1 / Vector3.Distance(transform.position, targetTransfrom.position));
    
    if (theRB.velocity == Vector2.zero)
    {
      fitness = 0;
    }
    fitness = dealtDamage;
    brain.fitness = fitness;

    if (Vector3.Distance(transform.position, targetTransfrom.position) > 30f)
      this.TakeDamage(9999f);
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= 0)
    {
      this.TakeDamage(9999f);
    }
  }

  public void FindAllies()
  {
    int count = 0;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visionRadius, allyMask);
    allies.Clear();
    foreach (Collider2D collider in colliders)
    {
      if (count <= NNManager.instance.countOfInputNeurons - 4)
      {
        allies.Add(collider.gameObject.transform.position);
      }

      count++;

    }
    Debug.Log(allies.Count);
  }


  public override void TakeDamage(float damage)
  {
    health -= damage;
    if (health <= 0f)
    {

      NNManager.instance.brainsFromGeneration.Add(brain);
      Destroy(gameObject);

      if (!NNManager.instance.turnOnNeuralNetworkEducation)
      {
        ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);

        if (Random.value <= coinDropRate)
        {
          CoinController.instance.DropCoin(transform.position, coinValue);
        }
      }

      SFXManager.instance.PlayeSFXPitched(0);
    }
    else
    {
      SFXManager.instance.PlayeSFXPitched(1);
    }

    DamageNumberController.instance.SpawnDamageNumber(damage, transform.position);
  }

}
