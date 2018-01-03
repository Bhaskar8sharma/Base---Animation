using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public int numUnits = 10;
    public float spawnRadius = 10;

    [Range(0, 20)]
    public int neighborDistance = 4;

    [Range(0, 2)]
    public float maxForce = 0.5f;

    [Range(0, 20)]
    public float maxVelocity = 4.0f;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numUnits; i++)
        {
            Vector3 unitPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            GameObject unit = Instantiate(unitPrefab, this.transform.position + unitPosition, Quaternion.identity) as GameObject;
            unit.transform.parent = this.gameObject.transform;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
