using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public struct Cube
{
    public Vector3 position;
    public Color color;
}

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;
    public Material screenMaterial;
    public RenderTexture renderTexture;
    // Start is called before the first frame update

    public void Start()
    {
        renderTexture = new RenderTexture(256, 256, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        updateRenderTexture();

        FullScreenCustomPass fullscreenPass = GetComponent<CustomPassVolume>().customPasses[0] as FullScreenCustomPass;

        fullscreenPass.fullscreenPassMaterial = new Material(screenMaterial);
        fullscreenPass.fullscreenPassMaterial.SetTexture("_MainTex" , renderTexture);
    }

    public void Update()
    {
        updateRenderTexture();
    }

    private void updateRenderTexture()
    {
        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetFloat("Resolution", renderTexture.width);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
    }
}
