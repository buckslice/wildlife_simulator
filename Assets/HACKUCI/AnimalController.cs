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
        shadow = (GameObject)Instantiate(GameManager.instance.shadowPrefab, GameManager.instance.shadowHolder);
        ShadowTrackTransform stt = shadow.GetComponent<ShadowTrackTransform>();
        stt.tracked = tform;
        stt.tform.localScale = model.localScale;

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (gp.touching) {

            // get camera forward vector with no y component
            Vector3 xzCamForward = cam.forward;
            xzCamForward.y = 0.0f;
            xzCamForward.Normalize();

            Vector3 upVel = Vector3.up * rb.velocity.y; // save current y vel
            Vector3 xzVel = xzCamForward * gp.dir.y + cam.right * gp.dir.x;
            rb.velocity = xzVel * moveSpeed + upVel;
            tform.forward = xzVel;    // point transform in movement direction

            // flip sprite based on x vel
            //Vector3 scale = tform.localScale;
            //bool isFlipped = scale.x < 0.0f;
            //if (gp.dir.x > 0 && isFlipped) {
            //    scale.x = origScaleX;
            //    tform.localScale = scale;
            //} else if (gp.dir.x < 0 && !isFlipped) {
            //    scale.x = -origScaleX;
            //    tform.localScale = scale;
            //}

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
    
    void Update() {
        // definitely over complicated and bad
        Vector3 viewDir = tform.position - cam.position;
        viewDir.y = 0.0f;
        Vector3 viewRight = -Vector3.Cross(viewDir, Vector3.up);
        float angle = Vector3.Angle(viewRight, -tform.right);
        angle = Mathf.Clamp(angle, 30.0f, 150.0f);  // limit from going full front back
        angle *= Vector3.Dot(tform.forward, cam.right) > 0.0f ? -1.0f : 1.0f; 
        model.rotation = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.LookRotation(viewRight);

        // flip scale of sprite if reasons
        Vector3 scale = model.localScale;
        scale.z = Vector3.Dot(tform.right, cam.forward) > 0 ? -1.0f : 1.0f;
        model.localScale = scale;
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
