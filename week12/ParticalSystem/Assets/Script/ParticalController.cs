using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalController : MonoBehaviour {

    ParticleSystem ps;
    float myMin = 1.0f;
    float myMax = 1000.0f;
    float myBase = 1000.0f;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();

    public GameObject XFTarget;


    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        XFTarget = GameObject.FindGameObjectWithTag("flame");
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
        int exitNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        int insideNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        int outsideNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);

        Debug.Log("enter Num:" + enterNum);
        Debug.Log("exit Num:" + exitNum);
        Debug.Log("inside Num:" + insideNum);
        Debug.Log("outside Num:" + outsideNum);

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
    }
}
