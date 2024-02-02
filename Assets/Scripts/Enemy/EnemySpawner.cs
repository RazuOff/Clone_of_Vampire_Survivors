using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemySpawner : MonoBehaviour
{
  public GameObject enemyToSpawn;
  public float timeToSpawn;
  private float spawnCounter;

  public Transform minSpawn, maxSpawn;
  private Transform spawnTargetFollow;
  private float despwanDistance;
  public List<WaveInfo> waves;

  private int currentWave;
  private float waveCounter;

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
    //spawnCounter -= Time.deltaTime;

    //if(spawnCounter <= 0)
    //{
    //  spawnCounter = timeToSpawn;

    //  Instantiate(enemyToSpawn, SelectSpawnPoint() ,transform.rotation);
    //}
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

        if (spawnCounter <= 0)
        {
          spawnCounter = waves[currentWave].timeBetweenSpawns;
          Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);

        }
      }
    }

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
    currentWave++;

    if (currentWave >= waves.Count)
    {
      currentWave = waves.Count - 1;
    }
    waveCounter = waves[currentWave].waveLength;
    spawnCounter = waves[currentWave].timeBetweenSpawns;
  }


}
  [System.Serializable]
  public class WaveInfo
  {
    public GameObject enemyToSpawn;
    public float waveLength = 10f;
    public float timeBetweenSpawns = 1f;


  }