using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunmove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (50 * Time.deltaTime, 0, 0);
	}
}
