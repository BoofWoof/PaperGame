using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganOverworldScript : PartnerBaseScript
{
    public bool AbilityOn = false;
    public float Count = 0;
    public float CountPeriod = 3f;
    public float BaseSpread = 1.5f;
    public float SpreadVariance = 0.1f;

    public float LightRangeMax = 2f;
    private float LightRangeCurrent = 0f;
    public float LightRangeGrowthRate = 4f;
    private bool SwitchingState = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void UseAbility()
    {
        if (!SwitchingState)
        {
            Shader.SetGlobalVector("_TorchPosition", OverworldController.Player.transform.position);
            StartCoroutine(GrowTorchSize());
        }
    }

    public override void CleanupAbility()
    {
        base.CleanupAbility();
        /*
        Vector4 my_pos = new Vector4(0, -1000, 0, 1);
        Shader.SetGlobalVector("_TorchPosition", my_pos);
        */
    }

    private void AbilityUpdate()
    {
        /*
        if (AbilityOn)
        {
            GameObject Player = OverworldController.Player;
            Vector4 my_pos = new Vector4(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z, 1);
            Shader.SetGlobalVector("_TorchPosition", my_pos);
            Count += Time.deltaTime;
            if (Count > CountPeriod) Count = 0;
            Shader.SetGlobalFloat("_DisappearDistance", BaseSpread + SpreadVariance*Mathf.Cos(Count/CountPeriod * 2*Mathf.PI));
        }
        */
    }

    IEnumerator GrowTorchSize()
    {
        SwitchingState = true;
        while (LightRangeCurrent < LightRangeMax)
        {
            LightRangeCurrent += LightRangeGrowthRate * Time.deltaTime;
            Shader.SetGlobalFloat("_DisappearDistance", LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = LightRangeMax;
        Shader.SetGlobalFloat("_DisappearDistance", LightRangeCurrent);
        StartCoroutine(ShrinkTorchSize());
    }

    IEnumerator ShrinkTorchSize()
    {
        SwitchingState = true;
        while (LightRangeCurrent > 0)
        {
            LightRangeCurrent -= LightRangeGrowthRate * Time.deltaTime;
            Shader.SetGlobalFloat("_DisappearDistance", LightRangeCurrent);
            yield return 0;
        }
        LightRangeCurrent = 0;
        Shader.SetGlobalFloat("_DisappearDistance", LightRangeCurrent);
        SwitchingState = false;
    }
}
