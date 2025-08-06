using System.Numerics;
using UnityEngine;


public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;

    //knowing checkpoint objects, array [] and giving it a name checkpoints, if its public we put how much we want them but we can miss it that way 
    private Checkpoint[] checkpoints;

    public UnityEngine.Vector3 spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        // puting them all that exist, and puts them in the scene
        checkpoints = FindObjectsOfType<Checkpoint>();

        spawnPoint = PlayerController.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateCheckpoints()
    {
        //new function for loop, we need to call it somewhere - Checkpoint
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].ResetCheckpoint();
        }
    }

    public void SetSpawnPoint(UnityEngine.Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
