using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public float itemXOffset;
    public float itemXSize;
    public int visibleColumns;

    public float itemYOffset;
    public float itemYSize;
    public int visibleRows;

    private int currentRow = 0;

    private List<int> debugItemList;
    private List<List<GameObject>> gameObjectRow = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        debugItemList = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };
        for (int i = 0; i < visibleRows; i++)
        {
            int row = currentRow + i;
            List<GameObject> gameObjectCol = new List<GameObject>();
            for (int j = 0; j < visibleColumns; j++)
            {
                GameObject NewObj = new GameObject(); //Create the GameObject
                NewObj.AddComponent<CanvasRenderer>(); //Add the Image Component script
                Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                NewObj.transform.position = transform.position + new Vector3(itemXOffset*j, -itemYOffset*i, 0);
                NewObj.transform.localScale = new Vector3(itemXSize, itemYSize, 1);
                NewObj.transform.SetParent(transform);
                gameObjectCol.Add(NewObj);
            }
            gameObjectRow.Add(gameObjectCol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (List<GameObject> delrows in gameObjectRow)
        {
            foreach (GameObject delitems in delrows)
            {
                Destroy(delitems);
            }
        }

        gameObjectRow = new List<List<GameObject>>();
        for (int i = 0; i < visibleRows; i++)
        {
            int row = currentRow + i;
            List<GameObject> gameObjectCol = new List<GameObject>();
            for (int j = 0; j < visibleColumns; j++)
            {
                GameObject NewObj = new GameObject(); //Create the GameObject
                NewObj.AddComponent<CanvasRenderer>(); //Add the Image Component script
                Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                NewObj.transform.position = transform.position + new Vector3(itemXOffset * j, -itemYOffset * i, 0);
                NewObj.transform.localScale = new Vector3(itemXSize, itemYSize, 1);
                NewObj.transform.SetParent(transform);
                gameObjectCol.Add(NewObj);
            }
            gameObjectRow.Add(gameObjectCol);
        }
        */
    }
}
