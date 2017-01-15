using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    AnimalController ac;
    Rigidbody rb;
    float dashTime = 1.0f;

    // Use this for initialization
    void Start() {
        TopDownGamePad tdgp = GetComponent<TopDownGamePad>();
        tdgp.OnTap += OnTap;

        ac = GetComponent<AnimalController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        dashTime -= Time.deltaTime;
        if(dashTime < 0.0f) {
            ac.blockMovement = false;
        }
	}

    void OnTap() {
        if(dashTime > -1.0f) {
            return;
        }

        ac.blockMovement = true;

        rb.velocity = transform.forward * ac.moveSpeed * 2.5f;

        dashTime = 1.0f;
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.CompareTag("Ground") || col.collider.CompareTag("StaticGeometry")) {
            return;
        }
        AnimalController oac = col.collider.GetComponent<AnimalController>();
        if (oac && ac.blockMovement) {   // if dashing
            oac.Damage();
        }
    }
}
