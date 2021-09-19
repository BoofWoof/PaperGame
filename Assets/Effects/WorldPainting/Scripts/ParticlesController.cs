using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public Color paintColor;
    public Color nextPaintColor;

    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    public float strength = 1;
    public float hardness = 1;
    [Space]
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    public bool waitingToStart = false;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        //var pr = part.GetComponent<ParticleSystemRenderer>();
        //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
        //paintColor = c;
    }

    private void Update()
    {
        if ((part.particleCount == 0 || paintColor == nextPaintColor) && !part.isPlaying && waitingToStart)
        {
            waitingToStart = false;
            part.Play();
            paintColor = nextPaintColor;
            part.startColor = paintColor;
        }
    }

    public void StartEmitter()
    {
        waitingToStart = true;
    }

    public void StopEmitter()
    {
        waitingToStart = false;
        part.Stop();
    }

    public void RandomizePaintColor()
    {
        nextPaintColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    public void NextPaintColor(Color nextColor)
    {
        nextPaintColor = nextColor;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Paintable p = other.GetComponent<Paintable>();
        if (p != null)
        {
            for (int i = 0; i < numCollisionEvents; i++)
            {
                Vector3 pos = collisionEvents[i].intersection;
                float radius = Random.Range(minRadius, maxRadius);
                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            }
        }
    }
}