using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform.GetChild(0))
            if (Random.value < 0.15f)
            {
                int prefabIndex = (Random.value < 0.5f) ? 0 : 1;
                Instantiate(obstaclePrefabs[prefabIndex], child.position, Quaternion.identity, transform.GetChild(1));
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
