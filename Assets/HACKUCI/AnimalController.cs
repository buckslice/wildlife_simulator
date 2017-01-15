using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    public float moveSpeed = 1.0f;

    TopDownGamePad gp;
    Rigidbody rb;
    Transform model;
    Transform cam;
    Transform tform;
    float origScaleX;
    GameObject shadow;
    bool grounded = false;

    // Use this for initialization
    void Start() {
        gp = GetComponent<TopDownGamePad>();
        rb = GetComponent<Rigidbody>();
        tform = transform;
        model = tform.Find("Model");
        origScaleX = tform.localScale.x;
        cam = Camera.main.transform;

        gp.OnTap += OnTap;
        gp.OnDisconnect += OnDisconnect;
        gp.OnColorChanged += ColorChanged;

        // spawn a shadow object on the ground that follows animals
        shadow = (GameObject)Instantiate(GameManager.get.shadowPrefab, GameManager.get.shadowHolder);
        shadow.GetComponent<ShadowTrackTransform>().tracked = tform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (gp.touching) {
            // get camera forward vector with no y component
            Vector3 camForward = cam.forward;
            camForward.y = 0.0f;
            camForward.Normalize();

            Vector3 upVel = Vector3.up * rb.velocity.y; // save current y vel
            Vector3 xzVel = camForward * gp.dir.y + cam.right * gp.dir.x;
            rb.velocity = xzVel * moveSpeed + upVel;
            tform.forward = xzVel;    // point transform in movement direction

            // flip sprite based on x vel
            Vector3 scale = tform.localScale;
            bool isFlipped = scale.x < 0.0f;
            if (gp.dir.x > 0 && isFlipped) {
                scale.x = origScaleX;
                tform.localScale = scale;
            } else if (gp.dir.x < 0 && !isFlipped) {
                scale.x = -origScaleX;
                tform.localScale = scale;
            }

        } else {
            Vector3 vel = rb.velocity;
            vel.x = 0.0f;
            vel.z = 0.0f;
            rb.velocity = vel;
        }
        
        // check if grounded for bunny
        if (Physics.SphereCast(new Ray(tform.position + Vector3.up * 0.3f, Vector3.down), 0.25f, 0.1f)
            && rb.velocity.y < 1.0f) {
            grounded = true;
        }

        //Debug.Log(gamepad.dir);    
    }

    void OnCollisionEnter(Collision c) {

    }

    void OnTap() {

        if (grounded) {
            //Debug.Log("Someone tapped! " + Time.time);
            Vector3 vel = rb.velocity;
            vel.y = 8.0f;
            rb.velocity = vel;
            grounded = false;
        }
    }

    void ColorChanged(Color c) {
        //sr.color = c;
    }

    void OnDisconnect() {
        Destroy(shadow);
        Destroy(gameObject);
    }
}
