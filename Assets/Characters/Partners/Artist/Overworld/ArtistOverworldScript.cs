using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistOverworldScript : PartnerBaseScript
{
    public GameObject particleGunPrefab;
    private GameObject particleGun;

    public float holdLengthThreshold = 0.3f;

    private float currentShotVerticalAngle = -20f;
    private float holdLength = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        particleGun = Instantiate(particleGunPrefab, OverworldController.Player.transform.position, OverworldController.Player.transform.rotation);
        particleGun.transform.parent = OverworldController.Player.transform;
        particleGun.GetComponent<ParticleSystem>().Stop();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Destroy(particleGun);
    }

    public override void UseAbility()
    {
        base.UseAbility();
        holdLength = 0;
        if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile)
        {
            GameDataTracker.gameModePre = GameDataTracker.gameMode;
            GameDataTracker.gameMode = GameDataTracker.gameModeOptions.AbilityFreeze;
            currentShotVerticalAngle = -20f;
            if (OverworldController.Player.GetComponent<SpriteFlipper>().goal > 90)
            {
                particleGun.transform.rotation = Quaternion.Euler(currentShotVerticalAngle, 90f, 0f);
            }
            else
            {
                particleGun.transform.rotation = Quaternion.Euler(currentShotVerticalAngle, -90f, 0f);
            }
        }
    }

    public override void HoldAbility()
    {
        base.HoldAbility();
        holdLength += Time.deltaTime;
        if (holdLength > holdLengthThreshold && !particleGun.GetComponent<ParticleSystem>().isPlaying)
        {
            particleGun.GetComponent<ParticlesController>().StartEmitter();
        }
        Vector2 stickPosition = controls.OverworldControls.Movement.ReadValue<Vector2>();
        float currentShotHorizontalAngle;
        if (OverworldController.Player.GetComponent<SpriteFlipper>().goal > 90)
        {
            currentShotVerticalAngle += Time.deltaTime * stickPosition[0] * 100f;
            currentShotHorizontalAngle = stickPosition[1] * -25f;
        }
        else
        {
            currentShotVerticalAngle -= Time.deltaTime * stickPosition[0] * 100f;
            currentShotHorizontalAngle = stickPosition[1] * 25f;
        }
        if (currentShotVerticalAngle > 20f) currentShotVerticalAngle = 20f;
        if (currentShotVerticalAngle < -90f)
        {
            currentShotVerticalAngle = -180 - currentShotVerticalAngle;
            if (OverworldController.Player.GetComponent<SpriteFlipper>().goal > 90)
            {
                OverworldController.Player.GetComponent<SpriteFlipper>().setFacingLeft();
            }
            else
            {
                OverworldController.Player.GetComponent<SpriteFlipper>().setFacingRight();
            }
        }
        if (OverworldController.Player.GetComponent<SpriteFlipper>().goal > 90)
        {
            particleGun.transform.rotation = Quaternion.Euler(currentShotVerticalAngle, 90f + currentShotHorizontalAngle, 0f);
        }
        else
        {
            particleGun.transform.rotation = Quaternion.Euler(currentShotVerticalAngle, -90f + currentShotHorizontalAngle, 0f);
        }
    }

    public override void AbilityReleased()
    {
        base.AbilityReleased();
        if (holdLength <= holdLengthThreshold)
        {
            particleGun.GetComponent<ParticlesController>().RandomizePaintColor();
        }
        particleGun.GetComponent<ParticlesController>().StopEmitter();
        if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.AbilityFreeze) GameDataTracker.gameMode = GameDataTracker.gameModePre;
        holdLength = 0;
    }
}
