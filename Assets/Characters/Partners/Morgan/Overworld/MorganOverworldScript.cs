using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganOverworldScript : PartnerBaseScript
{
    public bool AbilityOn = false;
    public Material MorganMaterial;

    public override void Start()
    {
        base.Start();
        Vector4 my_pos = new Vector4(0, -1000, 0, 1);
        MorganMaterial.SetVector("TorchPosition", my_pos);
    }

    public override void Update()
    {
        base.Update();
        AbilityUpdate();
    }

    public override void UseAbility()
    {
        AbilityOn = !AbilityOn;
        if (!AbilityOn)
        {
            Vector4 my_pos = new Vector4(0, -1000, 0, 1);
            MorganMaterial.SetVector("TorchPosition", my_pos);
        }
    }

    private void AbilityUpdate()
    {
        if (AbilityOn)
        {
            GameObject Player = OverworldController.Player;
            Vector4 my_pos = new Vector4(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z, 1);
            MorganMaterial.SetVector("TorchPosition", my_pos);
        }
    }
}
