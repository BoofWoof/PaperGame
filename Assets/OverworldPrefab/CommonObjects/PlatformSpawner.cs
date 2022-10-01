using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] PossiblePlatforms;
    public GameObject[] WaypointArray;
    public int[] WaypointOrder;
    public float Speed = 2;

    public float[] timeTillSpawn = new float[]{2f};
    public float spawnLoopPeriod = 4f;

    private float currentTime = 0f;
    private int currentSpawnIdx = 0;

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if(currentSpawnIdx < timeTillSpawn.Length)
        {
            if (timeTillSpawn[currentSpawnIdx] < currentTime)
            {
                GameObject newPlatform = Instantiate(PossiblePlatforms[Random.Range(0, PossiblePlatforms.Length)], transform.position, Quaternion.identity);
                ObjectWaypointFollower waypointFollower = newPlatform.GetComponent<ObjectWaypointFollower>();
                waypointFollower.WaypointArray = WaypointArray;
                waypointFollower.WaypointOrder = WaypointOrder;
                waypointFollower.Speed = Speed;
                waypointFollower.DeleteAtEnd = true;
                currentSpawnIdx += 1;
            }
        }
        if(currentTime > spawnLoopPeriod)
        {
            currentTime -= spawnLoopPeriod;
            currentSpawnIdx = 0;
        }
    }
}
