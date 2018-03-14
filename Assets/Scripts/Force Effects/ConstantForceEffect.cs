using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class ConstantForceEffect : MonoBehaviour {

	public string Type;
	public int effect_index;
	public float gain;
	public float magnitude;
	public float duration;
	public float frequency;
	public float[] positionEffect =  new float[3];
	public float[] directionEffect = new float[3];


    // Use this for initialization
    void Awake()
    {
		Type = "constant";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
