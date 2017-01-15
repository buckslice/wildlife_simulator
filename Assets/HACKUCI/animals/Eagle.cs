using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour {

    const float cruisingAltitude = 4.0f;
    const float landingAltitude = 0.2f;

    // Use this for initialization
    void Start() {
        TopDownGamePad tdgp = GetComponent<TopDownGamePad>();
        tdgp.OnTap += OnTap;
        tdgp.OnDeath += OnDeath;
    }

    // Update is called once per frame
    void Update() {
        Vector3 p = transform.position;
        p.y = cruisingAltitude;
        transform.position = p;
    }

    bool diving = false;
    void OnTap() {
        if (diving) {
            return;
        }
        diving = true;

        StartCoroutine(DiveRoutine());
    }

    void OnDeath() {
        Destroy(this);
    }

    float curAlt;
    IEnumerator DiveRoutine() {

        float t = 0.0f;
        Vector3 p = Vector3.zero;
        while (t < 1.0f) {
            p = transform.position;
            p.y = Mathf.Lerp(cruisingAltitude, landingAltitude, t);
            transform.position = p;
            t += Time.deltaTime;
            yield return null;
        }

        t = 0.0f;
        while (t < 1.0f) {
            p = transform.position;
            p.y = Mathf.Lerp(landingAltitude, cruisingAltitude, t);
            transform.position = p;
            t += Time.deltaTime;
            yield return null;
        }
        p.y = cruisingAltitude;
        transform.position = p;

        diving = false;
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.CompareTag("Ground") || col.collider.CompareTag("StaticGeometry")) {
            return;
        }
        AnimalController ac = col.collider.GetComponent<AnimalController>();
        if (ac) {
            ac.Damage();
        }
    }
}
