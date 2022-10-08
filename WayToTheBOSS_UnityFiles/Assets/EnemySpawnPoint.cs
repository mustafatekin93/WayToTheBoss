using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float respawnDistance;

    GameObject player;
    GameObject enemy;
    float distance;
    bool spawned = false;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player == null)
            return;

        distance = (player.transform.position - transform.position).magnitude;

        if (spawned == false && distance <= spawnDistance)
        {

            int index = Random.Range(0, prefabs.Length);
            enemy = (GameObject)Instantiate(prefabs[index], transform.position, Quaternion.identity);
            spawned = true;
        }

        else if (distance > respawnDistance && enemy == null)
        {
            spawned = false;
        }
    }
}
