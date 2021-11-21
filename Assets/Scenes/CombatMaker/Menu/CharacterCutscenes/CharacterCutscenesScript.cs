using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCutscenesScript : MonoBehaviour
{
    public GameObject SourceObject;

    private void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void Close()
    {
        SourceObject.SetActive(true);
        Destroy(gameObject);
    }
}
