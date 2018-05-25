using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalController : MonoBehaviour {

    ParticleSystem ps;
    float myMin = 100.0f;
    float myMax = 1000.0f;
    float myBase = 1000.0f;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        var em = ps.emission;
        float scale = em.rateOverTime.constantMax;
        if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            scale += (float)Input.GetAxis("Mouse ScrollWheel") * myBase;
            if (scale < myMin)
                scale = myMin;
            else if (scale > myMax)
                scale = myMax;
        }
        em.rateOverTime = new ParticleSystem.MinMaxCurve(scale);
	}
}
