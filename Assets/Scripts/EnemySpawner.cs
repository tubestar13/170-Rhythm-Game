using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    public string marker;

    public FMODVibeTracker vt;
    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        //Check for new marker
        if (marker != vt.timelineInfo.lastMarker)
        {
            //if they are not the same
            marker = vt.timelineInfo.lastMarker;
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
    }

    IEnumerator SpawnWave(Wave waveToSpawn)
    {
        Debug.Log("Spawning wave: " + waveToSpawn.name);
        state = SpawnState.SPAWNING;

        // Spawn
        for(int i = 0; i < waveToSpawn.count; i++)
        {
            SpawnEnemy(waveToSpawn.enemy);
            yield return new WaitForSeconds(1f / waveToSpawn.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy (Transform enemyToSpawn)
    {
        Debug.Log("Spawning Enemy: " + enemyToSpawn.name);
        Instantiate(enemyToSpawn, transform.position, transform.rotation);
    }
}
