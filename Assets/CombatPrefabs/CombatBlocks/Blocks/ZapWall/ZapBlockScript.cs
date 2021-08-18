using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapBlockScript : BlockTemplate
{
    public enum Direction {Left, Right, Up, Down};

    public Material CrystalOff;
    public Material CrystalOn;
    public GameObject CornerStructure;

    public Renderer DangerIndicator;

    public GameObject[] CornerStructures;

    public bool LeftOn;
    public Renderer[] LeftWallLights;
    public Renderer LeftWarning;
    public bool RightOn;
    public Renderer[] RightWallLights;
    public Renderer RightWarning;
    public bool UpOn;
    public Renderer[] UpperWallLights;
    public Renderer UpperWarning;
    public bool DownOn;
    public Renderer[] LowerWallLights;
    public Renderer LowerWarning;

    private float count = 0;
    private float count2 = 0;
    private bool light_toggle = true;

    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        SetLightDirection(Direction.Up, false);
        SetLightDirection(Direction.Down, false);
        SetLightDirection(Direction.Left, false);
        SetLightDirection(Direction.Right, false);
        ChangeDangerIndicatorLight(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        count += Time.deltaTime;
        if (count >= 6.3f && count < 7)
        {
            count2 += Time.deltaTime;
            if(count2 >= 0.2f)
            {
                if (light_toggle)
                {
                    SetLightDirection(Direction.Up, false, false, false);
                    SetLightDirection(Direction.Down, false, false, false);
                    SetLightDirection(Direction.Left, false, false, false);
                    SetLightDirection(Direction.Right, false, false, false);
                }
                else {
                    SetLightDirection(Direction.Up, UpOn, false, false);
                    SetLightDirection(Direction.Down, DownOn, false, false);
                    SetLightDirection(Direction.Left, LeftOn, false, false);
                    SetLightDirection(Direction.Right, RightOn, false, false);
                }
                count2 = 0;
                light_toggle = !light_toggle;
            }
        }
        if (count >= 7)
        {
            SetLightDirection(Direction.Up, (Random.value > 0.5f));
            SetLightDirection(Direction.Down, (Random.value > 0.5f));
            SetLightDirection(Direction.Left, (Random.value > 0.5f));
            SetLightDirection(Direction.Right, (Random.value > 0.5f));
            ChangeDangerIndicatorLight((Random.value > 0.5f));
            count = 0;
            count2 = 0;
            light_toggle = true;
        }
    }

    private void ForceWallAgreement(ZapBlockScript target_block_script, Direction source_direction, bool on) {
        if (source_direction == Direction.Up)
        {
            target_block_script.SetLightDirection(Direction.Down, on, true, false);
            return;
        }
        if (source_direction == Direction.Down)
        {
            target_block_script.SetLightDirection(Direction.Up, on, true, false);
            return;
        }
        if (source_direction == Direction.Left)
        {
            target_block_script.SetLightDirection(Direction.Right, on, true, false);
            return;
        }
        if (source_direction == Direction.Right)
        {
            target_block_script.SetLightDirection(Direction.Left, on, true, false);
            return;
        }
    }

    private ZapBlockScript findOtherZapBlock(Vector2Int posDif)
    {
        Vector2Int new_pos = pos + posDif;
        if (BattleMapProcesses.isThisOnTheGrid(new_pos))
        {
            GameObject target_block = ContainingGrid[new_pos.x, new_pos.y];
            return target_block.GetComponent<ZapBlockScript>();
        }
        return null;
    }

    public void SetLightDirection(Direction direction, bool on, bool record_change = true, bool force_agreement = true)
    {
        if (direction == Direction.Up)
        {
            ZapBlockScript target_block = findOtherZapBlock(new Vector2Int(0, 1));
            if(target_block != null)
            {
                ChangeLightList(UpperWallLights, on);
                ChangeIndicatorLight(UpperWarning, on);
                if (record_change) UpOn = on;
                if (force_agreement) ForceWallAgreement(target_block, direction, on);
            } else
            {
                ChangeLightList(UpperWallLights, false);
                ChangeIndicatorLight(UpperWarning, false);
                if (record_change) UpOn = false;
            }
            return;
        }
        if (direction == Direction.Down)
        {
            ZapBlockScript target_block = findOtherZapBlock(new Vector2Int(0, -1));
            if (target_block != null)
            {
                ChangeLightList(LowerWallLights, on);
                ChangeIndicatorLight(LowerWarning, on);
                if (record_change) DownOn = on;
                if (force_agreement) ForceWallAgreement(target_block, direction, on);
            }
            else
            {
                ChangeLightList(LowerWallLights, false);
                ChangeIndicatorLight(LowerWarning, false);
                if (record_change) DownOn = false;
            }
            return;
        }
        if (direction == Direction.Left)
        {
            ZapBlockScript target_block = findOtherZapBlock(new Vector2Int(-1, 0));
            if (target_block != null)
            {
                ChangeLightList(LeftWallLights, on);
                ChangeIndicatorLight(LeftWarning, on);
                if (record_change) LeftOn = on;
                if (force_agreement) ForceWallAgreement(target_block, direction, on);
            }
            else
            {
                ChangeLightList(LeftWallLights, false);
                ChangeIndicatorLight(LeftWarning, false);
                if (record_change) LeftOn = false;
            }
            return;
        }
        if (direction == Direction.Right)
        {
            ZapBlockScript target_block = findOtherZapBlock(new Vector2Int(1, 0));
            if (target_block != null)
            {
                ChangeLightList(RightWallLights, on);
                ChangeIndicatorLight(RightWarning, on);
                if (record_change) RightOn = on;
                if (force_agreement) ForceWallAgreement(target_block, direction, on);
            }
            else
            {
                ChangeLightList(RightWallLights, false);
                ChangeIndicatorLight(RightWarning, false);
                if (record_change) RightOn = false;
            }
            return;
        }
    }

    private void ChangeLightList(Renderer[] wallLights, bool on)
    {
        foreach(Renderer light in wallLights)
        {
            Material[] light_materials = light.materials;
            if (on) light_materials[1] = CrystalOn;
            else light_materials[1] = CrystalOff;
            light.materials = light_materials;
        }
    }

    private void ChangeIndicatorLight(Renderer indicatorLight, bool on)
    {
        if (on) indicatorLight.material = CrystalOn;
        else indicatorLight.material = CrystalOff;
    }

    private void ChangeDangerIndicatorLight(bool on)
    {
        if (on) DangerIndicator.material = CrystalOn;
        else DangerIndicator.material = CrystalOff;
    }

}
