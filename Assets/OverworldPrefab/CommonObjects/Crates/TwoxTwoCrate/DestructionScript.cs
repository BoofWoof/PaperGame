using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour
{
    public GameObject sourcePosition;
    public GameObject destroyedObject;

    public void BreakObject()
    {
        Instantiate(destroyedObject, sourcePosition.transform.position, sourcePosition.transform.rotation);
        Destroy(sourcePosition);
    }
}
