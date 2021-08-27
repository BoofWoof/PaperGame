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

    //TorchIDs
    public int TorchNumber;
    private string TorchDistanceName;
    private string TorchPositionName;

    public GameObject ExpansionBubblePrefab;
    private GameObject ExpansionBubble;
    
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
        TorchNumber = OverworldController.AllMorganTorches.Count;
        TorchDistanceName = "_ExtraTorch" + TorchNumber.ToString() + "DisappearDistance";
        TorchPositionName = "_ExtraTorch" + TorchNumber.ToString() + "Position";
        VFXSystem.Stop();
        TorchLight.intensity = 0;
        Shader.SetGlobalFloat(TorchDistanceName, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector(TorchPositionName, transform.position);
        if (controls.OverworldControls.PartnerAction.triggered && !SwitchingState)
        {
            if(Vector3.Distance(DistanceSource.transform.position, OverworldController.Player.transform.position) < Activation_Distance)
            {
                TorchOn = !TorchOn;
                if (TorchOn)
                {
                    VFXSystem.Play();
                    TorchLight.intensity = LightIntensityOn;
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
        ExpansionBubble = Instantiate(ExpansionBubblePrefab, transform.position, Quaternion.identity);
        ExpansionBubble.transform.parent = transform;
        SwitchingState = true;
        while (LightRangeCurrent < LightRangeMax)
        {
            LightRangeCurrent += LightRangeGrowthRate * Time.deltaTime;
            //Dividing by localscale is a rough way of scaling the ring.  Better solution recommended.
            ExpansionBubble.transform.localScale = Vector3.one * LightRangeCurrent * 2 / transform.localScale.x;
            Shader.SetGlobalFloat(TorchDistanceName, LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = LightRangeMax;
        Shader.SetGlobalFloat(TorchDistanceName, LightRangeCurrent);
        SwitchingState = false;
    }

    IEnumerator ShrinkTorchSize()
    {
        SwitchingState = true;
        while (LightRangeCurrent > 0)
        {
            LightRangeCurrent -= LightRangeGrowthRate * Time.deltaTime;
            ExpansionBubble.transform.localScale = Vector3.one * LightRangeCurrent * 2;
            Shader.SetGlobalFloat(TorchDistanceName, LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = 0;
        Shader.SetGlobalFloat(TorchDistanceName, LightRangeCurrent);
        SwitchingState = false;
        Destroy(ExpansionBubble);
    }
}
