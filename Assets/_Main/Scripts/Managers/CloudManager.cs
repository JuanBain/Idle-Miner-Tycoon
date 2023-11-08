using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;

    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        SpawnCloud();
    }

    private void SpawnCloud()
    {
        GameObject newCloud = Instantiate(cloudPrefab, spawnPosition.position, Quaternion.identity);
        Clouds cloud = newCloud.GetComponent<Clouds>();
        cloud.SpawnPosition = spawnPosition.position;
    }
}