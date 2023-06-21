using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private Transform spawnPoint;

    public void Spawn()
    {
        GameObject newObj = Instantiate(spawn);
        newObj.transform.position = spawnPoint.position;
    }
}
