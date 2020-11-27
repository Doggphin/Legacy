using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public Rigidbody rb;
    public Transform hand;
    public Transform player;

    public bool hasHit = false;
    public float initialLength;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 25f;
    }

    
    void Update()
    {
        if (hasHit)
        {
            Debug.DrawLine(this.transform.position, hand.transform.position, Color.white, -1f, true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hook hit!");
        rb.Sleep();
        initialLength = Vector3.Magnitude(this.transform.position - player.transform.position) * 1.05f;
        hasHit = true;
    }
}
