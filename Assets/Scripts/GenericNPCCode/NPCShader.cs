using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.receiveShadows = true;
        sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
}
