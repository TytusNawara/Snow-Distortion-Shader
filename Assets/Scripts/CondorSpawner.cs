using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondorSpawner : MonoBehaviour
{
    public GameObject plane;
    public GameObject condorPrefab;
    public float maxDistanceOfCondorSpawn = 60f;

    float timePassedSinceLastSpawn = 0f;
    float timeBetweenSpawns = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassedSinceLastSpawn += Time.deltaTime;
        if (timePassedSinceLastSpawn > timeBetweenSpawns) {
            spawnCondor();
            timePassedSinceLastSpawn = 0f;
        }
    }

    float myRand() {

        float toReturn = Random.Range(10f, 40f);
        if (Random.Range(0f, 1f) > 0.5f)
            toReturn *= -1;
        return toReturn;
    }

    void spawnCondor() {
        float randomX = Random.Range(plane.transform.position.x - plane.transform.localScale.x / 2, plane.transform.position.x + plane.transform.localScale.x / 2);
        float randomY = Random.Range(plane.transform.position.y - plane.transform.localScale.y / 2, plane.transform.position.y + plane.transform.localScale.y / 2);
        float randomZ = Random.Range(plane.transform.position.y - plane.transform.localScale.z / 2, plane.transform.position.y + plane.transform.localScale.z / 2);
        Vector3 randomPointOnPlane = new Vector3(randomX* 4f, randomY, randomZ * 4f);

        Vector3 randomSpawnPoint = transform.position;
        randomSpawnPoint.x += myRand();
        randomSpawnPoint.z += myRand();

        Vector3 condorFacingDirection = randomPointOnPlane - randomSpawnPoint;
        Quaternion rotation = Quaternion.LookRotation(condorFacingDirection, Vector3.up);
        GameObject instantiatedCondor =  Instantiate(condorPrefab, randomSpawnPoint, rotation);
        instantiatedCondor.GetComponent<ObjectInteratingWithSnow>().snowPlane = plane;
    }
}
