using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSwitcher : MonoBehaviour
{
    public SphereCollider groundedCheck;
    public List<CapsuleCollider> states;
    private CapsuleCollider currentCollider;
    private CapsuleCollider usedCollider;
    public int startCollider;
    private int currentIndex;

    private float lerpSpeed;

    private void Awake()
    {
        usedCollider = GetComponent<CapsuleCollider>();
        SwitchToCollider(startCollider);
        currentIndex = startCollider;
    }

    public void SwitchToCollider(int index)
    {
        currentIndex = index;
        currentCollider = states[index];
        SetLerpColliderToCurrent();
    }

    public void SwitchToCollider(int index, float speed)
    {
        lerpSpeed = speed;
        currentIndex = index;
        currentCollider = states[index];
    }

    public Collider GetCollider(int index)
    {
        return states[index];
    }

    public Collider GetConcreteCollider()
    {
        return usedCollider;
    }

    private void FixedUpdate()
    {
        usedCollider.radius = Mathf.MoveTowards(usedCollider.radius, currentCollider.radius, lerpSpeed * Time.deltaTime);
        usedCollider.height = Mathf.MoveTowards(usedCollider.height, currentCollider.height, lerpSpeed * Time.deltaTime);
        usedCollider.center = Vector3.MoveTowards(usedCollider.center, currentCollider.center, lerpSpeed * Time.deltaTime);
        groundedCheck.center = usedCollider.center - Vector3.up * (usedCollider.height/2f - usedCollider.radius);
    }

    private void SetLerpColliderToCurrent()
    {
        usedCollider.radius = currentCollider.radius;
        usedCollider.height = currentCollider.height;
        usedCollider.center = currentCollider.center;
        groundedCheck.center = usedCollider.center - Vector3.up * (usedCollider.height / 2f - usedCollider.radius);
    }
}
