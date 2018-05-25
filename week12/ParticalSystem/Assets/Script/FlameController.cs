using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour {
    public ParticleSystem ps;
    // Use this for initialization
    void Start () {
        ps = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeWithWater(int total)
    {
        Debug.Log(total);
        var main = ps.main;
        if (total <= 1000)
        {
            main.startLifetime = (float)0.002 * total + 2;
        }
        else if (total > 1000 && total <= 3000 && main.startLifetime.constantMax-0.0f>0.001f)
        {
            main.startLifetime = (float)-0.002 * total + 6;
            if(main.startLifetime.constantMax - 0.0f < 0.03f)
            {
                main.startLifetime = 0;
            }
        }
    }
}
