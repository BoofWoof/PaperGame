using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWaypointFollower : MonoBehaviour
{
    public GameObject[] WaypointArray;
    public int[] WaypointOrder;
    private int TargetIndex = 1;
    private bool ReadyToMove = true;

    public float Speed = 2;
    public float WaitSecEndpoint = 1;
    public bool DeleteAtEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = WaypointArray[WaypointOrder[0]].transform.position;
        tag = "MovingObject";
    }

    // Update is called once per frame
    void Update()
    {
        if(ReadyToMove) StartCoroutine(MoveToNextPos(WaypointArray[WaypointOrder[TargetIndex]].transform.position));
    }

    IEnumerator MoveToNextPos(Vector3 targetPos)
    {
        ReadyToMove = false;
        while(Vector3.Distance(targetPos, transform.position) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
            yield return 0;
        }
        if (TargetIndex == WaypointOrder.Length-1 && DeleteAtEnd)
        {
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(WaitSecEndpoint);
            ReadyToMove = true;
            TargetIndex += 1;
            TargetIndex %= WaypointOrder.Length;
        }
    }
}
