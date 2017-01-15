using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour {

    bool grounded = false;
    Transform tform;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        tform = transform;
        rb = GetComponent<Rigidbody>();

        GetComponent<TopDownGamePad>().OnTap += OnTap;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        if (Physics.SphereCast(new Ray(tform.position + Vector3.up * 0.3f, Vector3.down), 0.25f, 0.1f)
            && rb.velocity.y < 1.0f) {
            grounded = true;
        }
    }

    void OnTap() {
        if (grounded) {
            Vector3 vel = rb.velocity;
            vel.y = 8.0f;
            rb.velocity = vel;
            grounded = false;
        }
    }
}
