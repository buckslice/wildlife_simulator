using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrackTransform : MonoBehaviour {

    public Transform tracked;
    public Transform tform;

	// Use this for initialization
	void Awake () {
        tform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 p = tracked.position;
        p.y = 0.01f;
        tform.position = p;
	}
}
