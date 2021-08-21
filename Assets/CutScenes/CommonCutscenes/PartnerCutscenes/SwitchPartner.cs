using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPartner : CutSceneClass
{
    public int partner_id;
    public bool done = false;
    int phase = 0;
    SpriteFlipper sfControl;
    float rotated;
    // Start is called before the first frame update
    private void Start()
    {
    }

    override public bool Activate()
    {
        
        active = true;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (phase == 0)
        {
            sfControl = OverworldController.Partner.GetComponent<SpriteFlipper>();
            rotated = sfControl.rotated;
            OverworldController.Partner.GetComponent<PartnerBaseScript>().exitSpin = true;
            phase += 1;
        }
        if (phase == 1)
        {
            if (sfControl.rotated == 90)
            {
                OverworldController.SwapPartner(partner_id);
                sfControl = OverworldController.Partner.GetComponent<SpriteFlipper>();
                sfControl.setSpecificGoalInstant(90);
                if(rotated > 90) sfControl.setFacingLeft();
                else sfControl.setFacingRight();
                sfControl.ForceUpdate();
                return true;
            }
        }
        return false;
    }
}
