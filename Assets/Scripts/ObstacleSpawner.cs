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
    public bool spawnObstacles;
    public float minSpawnTime, maxSpawnTime;

    public void BeginSpawningObstacles()
    {
        spawnObstacles = true;
        Invoke("SpawnObject", 5f);
    }

    public void StopSpawningObstacles()
    {
        spawnObstacles = false;
        CancelInvoke();
    }

    void SpawnObject()
    {
        if (!spawnObstacles) { return; }

        Vector3 newObstaclePos = new Vector3(mainCam.transform.position.x + spawnDist, Random.Range(minObsHeight, maxObsHeight), 0f);
        GameObject newObstacle = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)], newObstaclePos, Quaternion.identity, obstacleContainer.transform);
        float randScale = Random.Range(minObsSize, maxObsSize);
        newObstacle.transform.localScale = new Vector3(randScale, randScale, randScale);
        Collider2D[] objectsTouchingObstacle = Physics2D.OverlapBoxAll(newObstacle.transform.position, Vector2.one * 0.5f, 0f);
        foreach (Collider2D col in objectsTouchingObstacle)
        {
            /*
            //Maybe leave in so obstacles don't overlap
            if (col.gameObject.tag == "Planet")
            {
                Destroy(col.gameObject);
                Debug.Log("destroying planet", newObstacle);
            }
            */
            if (col.gameObject.tag == "Powerup")
            {
                Debug.Log("destroying planet", newObstacle);
                Destroy(newObstacle.gameObject);  
            }
        }
        Invoke("SpawnObject", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
