using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clouds : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public Vector2 SpawnPosition { get; set; }

    private void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * moveSpeed));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        transform.position = new Vector3(SpawnPosition.x, SpawnPosition.y + Random.Range(-0.5f, 0.5f), 1);
    }
}