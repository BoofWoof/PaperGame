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

    public Color[] abilityColors;
    private int currentColorIndex = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        particleGun = Instantiate(particleGunPrefab, OverworldController.Player.transform.position, OverworldController.Player.transform.rotation);
        particleGun.transform.parent = OverworldController.Player.transform;
        particleGun.GetComponent<ParticleSystem>().Stop();
        particleGun.GetComponent<ParticlesController>().paintColor = abilityColors[currentColorIndex];
        particleGun.GetComponent<ParticlesController>().nextPaintColor = abilityColors[currentColorIndex];
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Destroy(particleGun);
    }

    public override void Start()
    {
        base.Start();
        particleGun.GetComponent<ParticlesController>().paintColor = abilityColors[currentColorIndex];
        particleGun.GetComponent<ParticlesController>().nextPaintColor = abilityColors[currentColorIndex];
    }

    public override void Update()
    {
        base.Update();
        Vector3 rayOrgin = OverworldController.Player.transform.position;
        RaycastHit hit;
        int currentPaintFloor = -1;
        if (Physics.Raycast(rayOrgin, Vector3.down, out hit, 0.6f))
        {
            Paintable paintableScript = hit.collider.gameObject.GetComponent<Paintable>();
            if (paintableScript != null)
            {
                RenderTexture maskTexture = paintableScript.getExtend();
                Vector2 hitCoordinate = hit.textureCoord;

                RenderTexture singlePixelTexture = new RenderTexture(1, 1, 0);
                Graphics.CopyTexture(
                    maskTexture, 
                    0,
                    0,
                    (int)(hitCoordinate.x * 1024),
                    (int)(hitCoordinate.y * 1024),
                    1,
                    1,
                    singlePixelTexture,
                    0,
                    0,
                    0,
                    0
                    );
                RenderTexture.active = singlePixelTexture;

                Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);

                tex.ReadPixels(new Rect(0, 0, 1, 1), 0, 0);
                Color underPixel = tex.GetPixel(0,0);
                
                RenderTexture.active = null;
                singlePixelTexture.Release();

                if (underPixel.a > 0.8f)
                {
                    Vector3 underPixelVector = new Vector3(underPixel.r, underPixel.g, underPixel.b);
                    float smallestDistance = 10000;
                    int colorIdx = 0;
                    foreach (Color abilityColor in abilityColors)
                    {
                        Vector3 abilityColorVector = new Vector3(abilityColor.r, abilityColor.g, abilityColor.b);
                        float colorDistance = Vector3.Distance(underPixelVector, abilityColorVector);
                        if (colorDistance < smallestDistance)
                        {
                            smallestDistance = colorDistance;
                            currentPaintFloor = colorIdx;
                        }
                        colorIdx += 1;
                    }
                }
            }
        }
        Debug.Log(currentPaintFloor);
        if(currentPaintFloor == 1)
        {
            OverworldController.Player.GetComponent<CharacterMovementOverworld>().ForceJump(20);
        }
    }

    public override void UseAbility()
    {
        base.UseAbility();
        holdLength = 0;
        if (GameDataTracker.cutsceneMode == GameDataTracker.cutsceneModeOptions.Mobile)
        {
            GameDataTracker.paintgunActive = true;
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
            currentColorIndex += 1;
            if (currentColorIndex >= abilityColors.Length) currentColorIndex = 0;
            particleGun.GetComponent<ParticlesController>().NextPaintColor(abilityColors[currentColorIndex]);
        }
        particleGun.GetComponent<ParticlesController>().StopEmitter();
        GameDataTracker.paintgunActive = false;
        holdLength = 0;
    }
}
