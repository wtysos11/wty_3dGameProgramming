using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

    GameObject fire;
    GameObject water;
    ParticalController PC;
    FlameController FC;
	// Use this for initialization
	void Start () {
        fire = GameObject.FindGameObjectWithTag("flame");
        water = GameObject.FindGameObjectWithTag("water");
        PC = water.GetComponent<ParticalController>();
        FC = fire.GetComponent<FlameController>();
        myEnable();
	}

    private void myEnable()
    {
        ParticalController.OnChangeWithWater += FC.changeWithWater;
    }

    private void myDisable()
    {
        ParticalController.OnChangeWithWater -= FC.changeWithWater;
    }

    // Update is called once per frame
    void Update () {
		
	}


}
