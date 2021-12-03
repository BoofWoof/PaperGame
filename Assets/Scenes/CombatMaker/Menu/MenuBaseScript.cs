using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBaseScript : MonoBehaviour
{
    public GameObject SourceMenu;
    // Start is called before the first frame update
    public virtual void Start()
    {
        if(SourceMenu != null)
        {
            transform.SetParent(SourceMenu.transform.parent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public virtual void Close()
    {
        if (SourceMenu != null)
        {
            SourceMenu.SetActive(true);
        }
        Destroy(gameObject);
    }

    public void CloseChain(int layers = 1)
    {
        if(SourceMenu != null)
        {
            SourceMenu.SetActive(true);
            if (layers <= 1)
            {
                SourceMenu.GetComponent<MenuBaseScript>().Close();
            } else
            {
                SourceMenu.GetComponent<MenuBaseScript>().CloseChain(layers-1);
            }
        }
        Close();
    }
}
