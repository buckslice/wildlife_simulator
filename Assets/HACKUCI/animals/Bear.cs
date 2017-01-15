using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour {

    public GameObject thrashObj;
    public Collider col;

    // Use this for initialization
    void Start() {
        GetComponent<TopDownGamePad>().OnTap += OnTap;
    }

    void OnTap() {
        Thrash();
    }

    bool thrashing = false;
    void Thrash() {
        if (!thrashing) {
            thrashing = true;
            StartCoroutine(ThrashRoutine());
        }
    }

    IEnumerator ThrashRoutine() {
        thrashObj.SetActive(true);
        Transform tf = thrashObj.transform;
        float t = 0.0f;
        while (t < 0.5f) {
            tf.localRotation = Quaternion.Euler(90.0f, 0.0f, Mathf.Lerp(-200.0f, 10.0f, t * 2.0f));
            t += Time.deltaTime;
            yield return null;
        }

        thrashObj.SetActive(false);
        thrashing = false;
    }

    void OnTriggerEnter(Collider other) {
        if (col == other || other.CompareTag("StaticGeometry")) {
            return;
        }
        TopDownGamePad tdgp = other.GetComponent<TopDownGamePad>();
        if (tdgp) {
            tdgp.RestartPlayer();
        }
    }
}
