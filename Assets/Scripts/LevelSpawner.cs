using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerGroup
{
    public string groupName;
    public GameObject prefab;
    public Transform[] spawnPoints;
}

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;
    [SerializeField] List<SpawnerGroup> spawnerGroups = new List<SpawnerGroup>();

    void Awake()
    {
        Instance = this;
    }
    public void Spawn()
    {
        foreach (var group in spawnerGroups)
        {
            foreach (var point  in group.spawnPoints)
            {
                Instantiate(group.prefab, point .position, Quaternion.identity);
            }
        }
    }
    
}
