using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class NNManager : MonoBehaviour
{


  public static NNManager instance;
  private void Awake()
  {
    instance = this;
  }

  public bool TurnOffMechanicsForNN = false;
  public float populationSize = 100f;
  public float mutationChance;
  public float maxLifetimeOfGeneration;
  public float mutationStrength;
  public float gameSpeed = 1f;


  public int numberOfGeneration = 0;
  private float oldSpeed;
  private float maxFitness;
  private float sceneMaxFitness;
  private int[] layers;
  private NeuralNetwork brain;
  public List<NeuralNetwork> brainsFromGeneration;
  public List<NeuralNetwork> brainsForNextGen;


  private void OnEnable()
  {
    EnemySpawner.onGenerationEnd += newGeneration;
  }
  private void OnDisable()
  {
    EnemySpawner.onGenerationEnd -= newGeneration;
  }

  private void Start()
  {
    sceneMaxFitness = 0f;
    brainsForNextGen = new List<NeuralNetwork>();
    brainsFromGeneration = new List<NeuralNetwork>();
    layers = new int[] { 4, 12, 12, 12, 12, 2 };

    for (int i = 0; i < populationSize; i++)
    {
      brain = new NeuralNetwork(layers);
      brainsForNextGen.Add(brain);
    }
  }


  private void FixedUpdate()
  {

    if (gameSpeed != oldSpeed)
    {
      Time.timeScale = gameSpeed;
      oldSpeed = gameSpeed;
    }
    float temp;
    if (brainsFromGeneration.Count != 0)
    {
      temp = Mathf.Max(brainsFromGeneration.Max(b => b.fitness));
    }
    else
      temp = 0f;
    if (maxFitness < temp)
      maxFitness = temp;
    UIController.instance.UpdateFitness(maxFitness);

    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    sceneMaxFitness = 0f;
    foreach (GameObject enemy in enemies)
    {
      float currFitness = enemy.GetComponent<EnemyNNController>().fitness;
      if (sceneMaxFitness < currFitness)
      {
        sceneMaxFitness = currFitness;
        UIController.instance.UpdateCurrentFitnessText(sceneMaxFitness);
      }

    }

  }
  private void newGeneration()
  {
    numberOfGeneration++;
    brainsFromGeneration.Sort(SortByFitness);
    brainsForNextGen.Clear();
    Mutation();
    brainsFromGeneration.Clear();

  }
  private void Mutation()
  {

    for (int i = 0; i < brainsFromGeneration.Count; i++)
    {
      if (3 > i)
      {
        Debug.Log(brainsFromGeneration[i].fitness);
        brainsForNextGen.Add(brainsFromGeneration[i]);
      }
      else if (brainsFromGeneration.Count * 0.25f > i)
      {
        brainsFromGeneration[i].Mutate(mutationChance, mutationStrength);
        brainsForNextGen.Add(brainsFromGeneration[i]);
      }
      else if (brainsFromGeneration.Count * 0.5f > i)
      {
        brainsFromGeneration[i].Mutate(mutationChance * 2, mutationStrength);
        brainsForNextGen.Add(brainsFromGeneration[i]);
      }
      else if (brainsFromGeneration.Count * 0.75f > i)
      {
        brainsFromGeneration[i].Mutate(mutationChance * 4, mutationStrength);
        brainsForNextGen.Add(brainsFromGeneration[i]);
      }
      else if (brainsFromGeneration.Count * 1f > i)
      {
        brainsFromGeneration[i].Mutate(mutationChance * 6, mutationStrength);
        brainsForNextGen.Add(brainsFromGeneration[i]);
      }
    }
  }


  private int SortByFitness(NeuralNetwork a, NeuralNetwork b)
  {
    return -(a.fitness.CompareTo(b.fitness));
  }



}
