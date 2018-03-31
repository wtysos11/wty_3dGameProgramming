using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetMove : MonoBehaviour {

	public Transform origin;
	public float speed;
	float ry,rx;
	float px,py;
	const int Kepler = 8000;

	// Use this for initialization
	void Start () {
		origin = GameObject.Find ("sun").transform;
		rx = Random.Range (10, 50);
		ry = Random.Range (10, 50);
		Debug.Log (this.transform.name.ToString()+":"+rx.ToString()+" "+ry.ToString());

		float nx,ny;
		nx = 1;
		ny = -1 * rx / ry;
		float dist = Vector3.Distance (origin.position, this.transform.position);
		float ratio = dist / Mathf.Sqrt (ny * ny + 1);
		px = 1 * ratio;
		py = ny * ratio;
		this.transform.position = new Vector3 (px, py, 0);

		speed = Mathf.Sqrt (Kepler / dist);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 axis = new Vector3 (rx, ry, 0);
		this.transform.RotateAround (origin.position, axis, speed * Time.deltaTime);
		this.transform.Rotate (0,50 * Time.deltaTime, 0);
	}
}
