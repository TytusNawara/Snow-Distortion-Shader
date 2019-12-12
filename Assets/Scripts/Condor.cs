using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condor : MonoBehaviour
{

    Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Vector3 v = transform.forward * Random.Range(20f, 60f);
        rb.velocity = v;
        Destroy(gameObject, 5);
    }

    private void FixedUpdate()
    {
        
    }
}
