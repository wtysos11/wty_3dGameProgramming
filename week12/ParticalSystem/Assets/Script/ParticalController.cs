using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalController : MonoBehaviour {

    public delegate void ChangeWithWater(int total);
    public static event ChangeWithWater OnChangeWithWater;

    ParticleSystem ps;
    float myMin = 1.0f;
    float myMax = 1000.0f;
    float myBase = 1000.0f;
    public int totalPartical = 0;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    public GameObject XFTarget;


    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        XFTarget = GameObject.FindGameObjectWithTag("flame");
        ps.trigger.SetCollider(0,XFTarget.GetComponent<BoxCollider>());
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

    void OnParticleTrigger()
    {
        int enterNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        totalPartical += enterNum;
        OnChangeWithWater(totalPartical);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
