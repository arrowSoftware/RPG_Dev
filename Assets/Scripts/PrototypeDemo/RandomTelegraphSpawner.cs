using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTelegraphSpawner : MonoBehaviour
{
    public float spawnInterval;
    public float spawnedItemLifetime;
    public List<GameObject> telegraphPrefabs = new List<GameObject>();

    Vector3 spawnArea;
    float currentTime = 0;

    void Start() {
        spawnArea = GetComponent<Collider>().bounds.size;
    }

    void Update() {
        currentTime += Time.deltaTime;
        
        if (currentTime > spawnInterval) {
            int randomTelegraph = Random.Range(0, telegraphPrefabs.Count);
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-spawnArea.x/2, spawnArea.x/2), 0.1f, Random.Range(-spawnArea.z/2, spawnArea.z/2));
            Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
            GameObject telegraph = Instantiate(telegraphPrefabs[randomTelegraph]);
            telegraph.transform.parent = transform;
            telegraph.transform.localPosition = randomSpawnPosition;
            telegraph.transform.rotation = Quaternion.Euler(randomRotation);
            //telegraph.name = "Floor_" + z + x ;
            telegraph.GetComponent<AOETelegraphScalar>().Begin(spawnedItemLifetime);
            currentTime = 0;
        }
    }
}
