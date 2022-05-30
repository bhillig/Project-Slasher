using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    [SerializeField] private float bounceForce; 
    private float timer = 0.0f; 
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        GameObject target = other.gameObject; 
        Rigidbody rb = target.GetComponent<Rigidbody>(); 
        rb.AddForce(Vector3.up * bounceForce);
        timer += Time.deltaTime; 
        if (timer > 0.5f) {
            rb.AddForce(Vector3.up * bounceForce);
            timer = 0;
        }
    }
}