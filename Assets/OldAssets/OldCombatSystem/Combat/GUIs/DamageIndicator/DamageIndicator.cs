using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public int damageAmount;

    private Vector3 endposition;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().text = damageAmount.ToString();
        endposition = transform.position + new Vector3(0.25f, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, endposition) < 0.25f*Time.deltaTime)
        {
            Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, endposition, 0.5f * Time.deltaTime);
    }
}
