using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movewithGravity : MonoBehaviour {

    Rigidbody rb;
	// Use this for initialization
	void Start () {
        Physics.gravity = new Vector3(0, -9.8f, 0);
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 20, 10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.Log(transform.position.ToString());
	}
}
