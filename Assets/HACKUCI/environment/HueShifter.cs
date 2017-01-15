using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShifter : MonoBehaviour {

    public Color shiftColor = Color.black;
    public Vector2 minMaxRando = Vector2.one;

	// Use this for initialization
	void Start () {
        MeshRenderer[] childRens = GetComponentsInChildren<MeshRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_Color", Color.Lerp(shiftColor, Color.white, Random.Range(minMaxRando.x, minMaxRando.y)));
        for(int i = 0; i < childRens.Length; ++i) {
            childRens[i].SetPropertyBlock(mpb);
        }
	}
	

}
