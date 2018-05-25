using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

    GameObject fire;
    GameObject water;
    GameObject fireTrigger;
	// Use this for initialization
	void Start () {
        fire = GameObject.FindGameObjectWithTag("flame");
        water = GameObject.FindGameObjectWithTag("water");
        fireTrigger = GameObject.FindGameObjectWithTag("fireTrigger");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
