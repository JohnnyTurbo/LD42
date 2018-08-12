using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    public GameObject[] obstaclePrefabs;
    public GameObject mainCam;
    public GameObject obstacleContainer;
    public float minObsHeight, maxObsHeight;
    public float spawnDist;
    public float minObsSize, maxObsSize;
    [Range(0f, 1f)]
    public float obstacleFrequency;
    public bool spawnObstacles;
    public float minSpawnTime, maxSpawnTime;

    /*
    private void Update()
    {
        if (spawnObstacles && Random.Range(0f, 1f) < obstacleFrequency)
        {
            Vector3 newObstaclePos = new Vector3(mainCam.transform.position.x + spawnDist, Random.Range(minObsHeight, maxObsHeight), 0f);
            GameObject newObstacle = Instantiate(obstaclePrefabs[0], newObstaclePos, Quaternion.identity, obstacleContainer.transform);
            float randScale = Random.Range(minObsSize, maxObsSize);
            newObstacle.transform.localScale = new Vector3(randScale, randScale, randScale);
        }
    }
    */

    private void Start()
    {
        //Invoke("SpawnObject", 1f);
    }

    public void BeginSpawningObstacles()
    {
        spawnObstacles = true;
        Invoke("SpawnObject", 5f);
    }

    void SpawnObject()
    {
        if (!spawnObstacles) { return; }

        Vector3 newObstaclePos = new Vector3(mainCam.transform.position.x + spawnDist, Random.Range(minObsHeight, maxObsHeight), 0f);
        GameObject newObstacle = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)], newObstaclePos, Quaternion.identity, obstacleContainer.transform);
        float randScale = Random.Range(minObsSize, maxObsSize);
        newObstacle.transform.localScale = new Vector3(randScale, randScale, randScale);

        Invoke("SpawnObject", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
