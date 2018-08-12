using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public GameObject mainCam;
    public GameObject powerupContainer;
    public float minPowerupHeight, maxPowerupHeight;
    public float spawnDist;
    public bool spawnPowerups;
    public float minSpawnTime, maxSpawnTime;

    public void BeginSpawningPowerups()
    {
        spawnPowerups = true;
        Invoke("SpawnPowerup", 10f);
    }

    public void StopSpawningPowerups()
    {
        spawnPowerups = false;
        CancelInvoke();
    }

    void SpawnPowerup()
    {
        if (!spawnPowerups) { return; }

        Vector3 newObstaclePos = new Vector3(mainCam.transform.position.x + spawnDist, Random.Range(minPowerupHeight, maxPowerupHeight), 0f);
        GameObject newPowerup = Instantiate(powerupPrefab, newObstaclePos, Quaternion.identity, powerupContainer.transform);
        Collider2D[] objectsTouchingPowerup = Physics2D.OverlapBoxAll(newPowerup.transform.position, Vector2.one * 0.5f, 0f);
        foreach(Collider2D col in objectsTouchingPowerup)
        {
            if(col.gameObject.tag == "Planet")
            {
                Destroy(col.gameObject);
                Debug.Log("destroying planet", newPowerup);
            }
        }
        Invoke("SpawnPowerup", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
