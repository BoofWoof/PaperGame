using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MorganTorchScript : MonoBehaviour
{
    public Light TorchLight;
    public VisualEffect VFXSystem;
    private GameControls controls;
    public float Activation_Distance = 1f;
    public float LightIntensityOn = 100000;
    public bool TorchOn = false;
    public float LightRangeMax = 5f;
    public float LightRangeCurrent = 0f;
    public float LightRangeGrowthRate = 2f;
    private bool SwitchingState = false;
    public GameObject DistanceSource;
    
    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    private void Start()
    {
        OverworldController.AllMorganTorches.Add(this);
        VFXSystem.Stop();
        TorchLight.intensity = 0;
        Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.OverworldControls.PartnerAction.triggered && !SwitchingState)
        {
            if(Vector3.Distance(DistanceSource.transform.position, OverworldController.Player.transform.position) < Activation_Distance)
            {
                TorchOn = !TorchOn;
                if (TorchOn)
                {
                    VFXSystem.Play();
                    TorchLight.intensity = LightIntensityOn;
                    Shader.SetGlobalVector("_ExtraTorch1Position", transform.position);
                    StartCoroutine(GrowTorchSize());
                } else
                {
                    VFXSystem.Stop();
                    TorchLight.intensity = 0;
                    StartCoroutine(ShrinkTorchSize());
                }
            }
        }
    }

    IEnumerator GrowTorchSize()
    {
        SwitchingState = true;
        while (LightRangeCurrent < LightRangeMax)
        {
            LightRangeCurrent += LightRangeGrowthRate * Time.deltaTime;
            Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = LightRangeMax;
        Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", LightRangeCurrent);
        SwitchingState = false;
    }

    IEnumerator ShrinkTorchSize()
    {
        SwitchingState = true;
        while (LightRangeCurrent > 0)
        {
            LightRangeCurrent -= LightRangeGrowthRate * Time.deltaTime;
            Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = 0;
        Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", LightRangeCurrent);
        SwitchingState = false;
    }
}
