using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    public float moveSpeed = 1.0f;
    public int health = 1;

    TopDownGamePad gp;
    Rigidbody rb;
    Transform model;
    Transform cam;
    Transform tform;
    GameObject shadow;
    bool grounded = false;

    // Use this for initialization
    void Start() {
        gp = GetComponent<TopDownGamePad>();
        rb = GetComponent<Rigidbody>();
        tform = transform;
        model = tform.Find("Model");
        cam = Camera.main.transform;

        gp.OnDisconnect += OnDisconnect;
        gp.OnDeath += OnDeath;
        gp.OnColorChanged += ColorChanged;

        // spawn a shadow object on the ground that follows animals x/z
        shadow = (GameObject)Instantiate(GameManager.instance.shadowPrefab, GameManager.instance.shadowHolder);
        ShadowTrackTransform stt = shadow.GetComponent<ShadowTrackTransform>();
        stt.tracked = tform;
        stt.tform.localScale = model.localScale;

        tform.forward = cam.right;  // start animals off facing right
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (dead) {
            return;
        }
        if (gp.touching) {
            // get camera forward vector with no y component
            Vector3 xzCamForward = cam.forward;
            xzCamForward.y = 0.0f;
            xzCamForward.Normalize();

            Vector3 upVel = Vector3.up * rb.velocity.y; // save current y vel
            Vector3 xzVel = xzCamForward * gp.dir.y + cam.right * gp.dir.x;
            rb.velocity = xzVel * moveSpeed + upVel;
            tform.forward = xzVel;    // point transform in movement direction

        } else {    // zero out x/z movement
            Vector3 vel = rb.velocity;
            vel.x = 0.0f;
            vel.z = 0.0f;
            rb.velocity = vel;
        }

    }
    
    void Update() {
        if (dead) {
            return;
        }
        // definitely over complicated and bad
        Vector3 viewDir = tform.position - cam.position;
        viewDir.y = 0.0f;
        Vector3 viewRight = -Vector3.Cross(viewDir, Vector3.up);
        float angle = Vector3.Angle(viewRight, -tform.right);
        angle = Mathf.Clamp(angle, 35.0f, 145.0f);  // limit from going full front back
        angle *= Vector3.Dot(tform.forward, cam.right) > 0.0f ? -1.0f : 1.0f; 
        model.rotation = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.LookRotation(viewRight);

        // flip scale of sprite if reasons
        Vector3 scale = model.localScale;
        scale.z = Vector3.Dot(tform.right, cam.forward) > 0 ? -1.0f : 1.0f;
        model.localScale = scale;
    }

    public void Damage() {
        health--;
        if (health <= 0) {
            gp.RestartPlayer();
        }
    }

    void ColorChanged(Color c) {
        //sr.color = c;
    }

    void OnDisconnect() {
        Destroy(shadow);
        Destroy(gameObject);
    }

    bool dead = false;
    void OnDeath() {
        rb.useGravity = true;
        dead = true;
        Vector3 scale = model.localScale;
        scale.y = -scale.y;
        model.localScale = scale;
        Destroy(shadow, 10.0f); // thats how long restart is
        //Destroy(this);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground") && dead) {
            rb.isKinematic = true;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.collider.CompareTag("Ground") && dead) {
            rb.isKinematic = true;
        }
    }
}
