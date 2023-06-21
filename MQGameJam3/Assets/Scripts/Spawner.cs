using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private Transform spawnPoint;

    private bool checkSpawn = true;
    private float lastSpawnCheck;

    void Update()
    {
        if (!Physics.Raycast(spawnPoint.position, Vector3.up, 3))
        {
            if (Time.time - lastSpawnCheck > 1)
            {
                lastSpawnCheck = Time.time;
                checkSpawn = !checkSpawn;
                if (checkSpawn)
                {
                    Spawn();
                }
            }
        }
    }

    public void Spawn()
    {
        GameObject newObj = Instantiate(spawn);
        newObj.transform.position = spawnPoint.position;
    }
}
