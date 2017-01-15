using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShifter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshRenderer[] childRens = GetComponentsInChildren<MeshRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_Color", Color.white * Random.Range(0.25f, 1.0f));
        for(int i = 0; i < childRens.Length; ++i) {
            childRens[i].SetPropertyBlock(mpb);
        }
	}
	

}
