using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour {

    Transform tform;
    Transform mainCam;

	// Use this for initialization
	void Start () {
        tform = transform;
        mainCam = Camera.main.transform;
	}

    void OnWillRenderObject() {
        Vector3 viewDir = tform.position - mainCam.position;
        viewDir.y = 0.0f;   // dont want it to tilt up probly
        transform.rotation = Quaternion.LookRotation(viewDir);
    }
}