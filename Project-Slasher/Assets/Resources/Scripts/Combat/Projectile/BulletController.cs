using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BulletController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float destructTime;
    [SerializeField] private float trackingAcceleration;
    [SerializeField] private float moveSpeed;
    private Transform player;
    private Collider coll;
    private Rigidbody rb;
    private bool destroyed = false;
    private float targetHeightOffset;
    private float timer = 0f;

    [Header("Renderers")]
    [SerializeField] private Renderer bulletRenderer;

    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> impactParticles;
    [SerializeField] private List<ParticleSystem> trailParticles;

    [Header("SFX")]
    [SerializeField] private FMODUnity.StudioEventEmitter emitter;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void SetStartDirection(Vector3 dir)
    {
        ProjectileMove(moveSpeed,dir);
    }

    public void SetTargetHeightOffset(float val)
    {
        targetHeightOffset = val;
    }

    private void FixedUpdate()
    {
        if (!destroyed)
        {
            float accelStep = trackingAcceleration * Time.deltaTime;
            Vector3 toPlayer = player.position + Vector3.up * targetHeightOffset - transform.position;
            ProjectileMove(accelStep,toPlayer);
        }    
    }

    private void ProjectileMove(float accelStep, Vector3 targetDir)
    {
        Vector3 currentVel = rb.velocity;
        Vector3 desiredVel = targetDir.normalized * moveSpeed;
        Vector3 newVel = Vector3.MoveTowards(currentVel, desiredVel, accelStep);
        rb.velocity = newVel;
        this.gameObject.transform.forward = rb.velocity;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > destructTime && !destroyed)
            OnImpact();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (destroyed)
            return;
        var player = collision.gameObject.GetComponentInParent<PlayerHitboxManager>();
        if (player != null)
        {
            player.HitByProjectile();
        }
        OnImpact();
    }

    private void OnImpact()
    {
        emitter.Stop();
        impactParticles.ForEach(particle => particle?.Play());
        trailParticles.ForEach(particle => particle?.Stop());
        rb.constraints = RigidbodyConstraints.FreezeAll;
        bulletRenderer.enabled = false;
        coll.enabled = false;
        destroyed = true;
        Destroy(gameObject, 0.5f);
    }
}
