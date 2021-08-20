using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSubMenu : MonoBehaviour
{
    public GameObject itemGenerator;
    ItemList itemGenList;
    // Start is called before the first frame update
    void Start()
    {
        itemGenList = itemGenerator.GetComponent<ItemList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMenuData()
    {
        itemGenList.clearItems();
        itemGenList.generateItems();
    }
}
