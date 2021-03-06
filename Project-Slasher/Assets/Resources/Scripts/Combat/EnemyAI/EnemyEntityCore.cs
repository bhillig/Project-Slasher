using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyEntityCore : AbstractEnemyEntity
{
    [Header("Components")]
    public List<Collider> colliders;
    public List<Renderer> renderers;
    public List<ParticleSystem> deathParticles;

    public override void KillEnemy()
    {
        isDead = true;
        colliders.ForEach(collider => collider.enabled = false);
        renderers.ForEach(collider => collider.enabled = false);
        deathParticles.ForEach(deathParticle => deathParticle.Play());
        OnDead?.Invoke();
    }

    public override void Respawn()
    {
        isDead = false; 
        colliders.ForEach(collider => collider.enabled = true);
        renderers.ForEach(collider => collider.enabled = true);
        OnRespawn?.Invoke();
    }

    [ContextMenu("Autofill fields")]
    public void AutoFillComponents()
    {
        colliders = new List<Collider>(gameObject.GetComponentsInChildren<Collider>());
        renderers = new List<Renderer>(gameObject.GetComponentsInChildren<Renderer>());
    }
}
