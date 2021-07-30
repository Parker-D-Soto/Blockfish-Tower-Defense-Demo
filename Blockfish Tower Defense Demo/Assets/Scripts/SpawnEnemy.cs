using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //Refers to the Enemy being spawned assigned in editor
    public Transform enemy;
    //Time Rate for Spawning
    public float spawnRate = 5.0f;
    //Time taken since last enemy was spawned
    private float timeSinceLastSpawn = 0.0f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        //Calculates time since last update and adds it to time since the last spawn occured
        timeSinceLastSpawn += Time.deltaTime;
        //Compares the time since the last spawn to the spawn rate
        //If successful spawns an enemy above the spawner Object Cube
        if(timeSinceLastSpawn >= spawnRate)
        {
            timeSinceLastSpawn = 0.0f;
            Transform newEnemy = Instantiate(enemy, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            newEnemy.gameObject.SetActive(true);
        }
    }
}
