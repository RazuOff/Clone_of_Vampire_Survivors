using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;



public class NNManager : MonoBehaviour
{


  public static NNManager instance;

  public int countOfInputNeurons = 4;
  public bool turnOnNeuralNetworkEducation = false;
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
  public List<NeuralNetwork> brainsForSave;
  private string path;

  private void Awake()
  {
    instance = this;
    path = Path.Combine(Application.persistentDataPath, "brains.json");

    sceneMaxFitness = 0f;
    brainsForNextGen = new List<NeuralNetwork>();
    brainsFromGeneration = new List<NeuralNetwork>();
    layers = new int[] { countOfInputNeurons, 12, 12, 12, 12, 2 };
    if (turnOnNeuralNetworkEducation)
    {
      for (int i = 0; i < populationSize; i++)
      {
        brain = new NeuralNetwork(layers);
        brainsForNextGen.Add(brain);
      }
    }
    else
    {
      LoadNeuralNetworks();
      Debug.Log(brainsForNextGen);

    }
  }

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
    //sceneMaxFitness = 0f;
    //brainsForNextGen = new List<NeuralNetwork>();
    //brainsFromGeneration = new List<NeuralNetwork>();
    //layers = new int[] { countOfInputNeurons, 12, 12, 12, 12, 2 };
    //if (turnOnNeuralNetworkEducation)
    //{
    //  for (int i = 0; i < populationSize; i++)
    //  {
    //    brain = new NeuralNetwork(layers);
    //    brainsForNextGen.Add(brain);
    //  }
    //}
    //else
    //{
    //  LoadNeuralNetworks();
    //  Debug.Log(brainsForNextGen);
      
    //}
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
    brainsForSave.Clear();
    brainsForSave.AddRange(brainsForNextGen);
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
    brainsForSave.Clear();
    brainsForSave.AddRange(brainsForNextGen);
  }


  private int SortByFitness(NeuralNetwork a, NeuralNetwork b)
  {
    return -(a.fitness.CompareTo(b.fitness));
  }
  
  public void SaveNetworks()
  { 
    brainsForSave.Sort(SortByFitness);
    NeuralNetworkListData listData = new NeuralNetworkListData(brainsForSave);
    string json = JsonUtility.ToJson(listData);
    File.WriteAllText(path, json);
    Debug.Log(json);

    Debug.Log("Saved");

  }

  public void LoadNeuralNetworks()
  {
    if (File.Exists(path))
    {
      string json = File.ReadAllText(path);
      NeuralNetworkListData listData = JsonUtility.FromJson<NeuralNetworkListData>(json);
      RestoreNeuralNetworks(listData);
    }
    else
    {
      Debug.LogError("Файл не найден: " + path);
    }
  }

  private void RestoreNeuralNetworks(NeuralNetworkListData listData)
  {
    brainsForNextGen.Clear();
    

    for (int i =0; i<listData.networkDataList.Count; i++ )
    {
      int indexOfBrain = Random.Range(0, 3);
      NeuralNetwork network = new NeuralNetwork
      {
        layers = listData.networkDataList[indexOfBrain].layers,
        neurons = UnflattenArray(listData.networkDataList[indexOfBrain].flatNeurons, listData.networkDataList[indexOfBrain].layers),
        biases = UnflattenArray(listData.networkDataList[indexOfBrain].flatBiases, listData.networkDataList[indexOfBrain].layers),
        weights = UnflattenWeights(listData.networkDataList[indexOfBrain].flatWeights, listData.networkDataList[indexOfBrain].layers),
        activations = listData.networkDataList[indexOfBrain].activations,
        fitness = listData.networkDataList[indexOfBrain].fitness
      };
      brainsForNextGen.Add(network);
    }

  }

  private static float[][] UnflattenArray(float[] flatArray, int[] lengths)
  {
    int index = 0;
    float[][] newArray = new float[lengths.Length][];
    for (int i = 0; i < lengths.Length; i++)
    {
      newArray[i] = new float[lengths[i]];
      for (int j = 0; j < lengths[i]; j++)
      {
        newArray[i][j] = flatArray[index++];
      }
    }
    return newArray;
  }

  private static float[][][] UnflattenWeights(float[] flatWeights, int[] lengths)
  {
    int index = 0;
    float[][][] newWeights = new float[lengths.Length - 1][][];
    for (int i = 0; i < lengths.Length - 1; i++)
    {
      newWeights[i] = new float[lengths[i + 1]][];
      for (int j = 0; j < lengths[i + 1]; j++)
      {
        newWeights[i][j] = new float[lengths[i]];
        for (int k = 0; k < lengths[i]; k++)
        {
          newWeights[i][j][k] = flatWeights[index++];
        }
      }
    }
    return newWeights;
  }
}
