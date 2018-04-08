using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movewithMoveTowards : MonoBehaviour {
    private Vector3 g;
    private Vector3 vy;
    private Vector3 vx;
    private float step;
	// Use this for initialization
	void Start () {
        step = Time.deltaTime;
        g = new Vector3(0, 9.8f, 0);
        vy = new Vector3(0, 50, 0);
        vx = new Vector3(0, 0, 30);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 target;
        target = this.transform.position + (vy + vx) * step; 
        this.transform.position=Vector3.MoveTowards(this.transform.position, target, step);
        vy -= g * step;
        Debug.Log(transform.position.ToString());
	}
}
