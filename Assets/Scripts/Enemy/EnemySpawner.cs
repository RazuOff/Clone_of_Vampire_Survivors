using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
  public static Action onGenerationEnd;


  public float timeToSpawn;
  private float spawnCounter;
  private float numberOfSpawned;

  public Transform minSpawn, maxSpawn;
  private Transform spawnTargetFollow;
  private float despwanDistance;
  public List<WaveInfo> waves;

  private List<NeuralNetwork> brainsFromLoad = new List<NeuralNetwork>();
  private int currentWave;
  private float waveCounter;
  private List<EnemyController> enemies = new List<EnemyController>();
  void Start()
  {
    spawnCounter = timeToSpawn;
    spawnTargetFollow = PlayerHealthController.instance.transform;

    despwanDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4f;

    currentWave = -1;
    GoToNextWave();
  }


  void Update()
  {
    transform.position = spawnTargetFollow.position;


    if (PlayerHealthController.instance.gameObject.activeSelf)
    {
      if (currentWave < waves.Count)
      {
        waveCounter -= Time.deltaTime;

        if (waveCounter <= 0)
        {
          GoToNextWave();
        }

        spawnCounter -= Time.deltaTime;

        if (!NNManager.instance.turnOnNeuralNetworkEducation)
        {
          if (numberOfSpawned < NNManager.instance.populationSize)
          {
            if (spawnCounter <= 0)
            {
              SpawnEnemy(brainsFromLoad);
            }
          }

        }
        else if (numberOfSpawned == NNManager.instance.populationSize)
        {
          if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
          {
            numberOfSpawned = 0;
            onGenerationEnd?.Invoke();
            UIController.instance.UpdateGeneration(NNManager.instance.numberOfGeneration);
          }

        }
        else if (spawnCounter <= 0)
        {
          SpawnEnemy(NNManager.instance.brainsForNextGen);
        }

      }
    }

  }

  private void SpawnEnemy(List<NeuralNetwork> brains)
  {
    spawnCounter = waves[currentWave].timeBetweenSpawns;
    GameObject enemy = Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);
    EnemyNNController enemyNNController = enemy.GetComponent<EnemyNNController>();
    enemyNNController.SetBrain(brains[0]);
    brains.RemoveAt(0);
    numberOfSpawned++;
  }


  public Vector3 SelectSpawnPoint()
  {
    Vector3 spawnPoint = Vector3.zero;

    bool verticalOrHorizontal = Random.value > 0.5f;


    if (verticalOrHorizontal)
    {
      spawnPoint = new Vector3(Random.Range(minSpawn.position.x, maxSpawn.position.x), Random.value < 0.5f ? minSpawn.position.y : maxSpawn.position.y, 0f);

    }
    else
      spawnPoint = new Vector3(Random.value < 0.5f ? minSpawn.position.x : maxSpawn.position.x, Random.Range(minSpawn.position.y, maxSpawn.position.y), 0f);


    return spawnPoint;

  }


  public void GoToNextWave()
  {
    if (false)
    {
      currentWave++;

      if (currentWave >= waves.Count)
      {
        currentWave = waves.Count - 1;
      }
      waveCounter = waves[currentWave].waveLength;
      spawnCounter = waves[currentWave].timeBetweenSpawns;
    }
    else
    {
      brainsFromLoad.AddRange(NNManager.instance.brainsForNextGen);
      currentWave = 0;
      waveCounter = waves[currentWave].waveLength;
      spawnCounter = waves[currentWave].timeBetweenSpawns;

      if (!NNManager.instance.turnOnNeuralNetworkEducation)
      {
        numberOfSpawned = 0;
      }
    }
  }


}
[System.Serializable]
public class WaveInfo
{
  public GameObject enemyToSpawn;
  public float waveLength = 10f;
  public float timeBetweenSpawns = 1f;


}