using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {
    GUIStyle buttonStyle;
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
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
    }

    // Update is called once per frame
    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 140, 70), "Restart", buttonStyle))
        {
            PC.totalPartical = 0;
            var main = FC.ps.main;
            main.startLifetime = 2;
        }
    }
}
