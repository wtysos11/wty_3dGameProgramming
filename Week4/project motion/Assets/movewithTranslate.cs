using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movewithTranslate : MonoBehaviour {
    private Vector3 g;
    private Vector3 vy;
    private Vector3 vx;
    private float step;
    // Use this for initialization
    void Start()
    {
        step = Time.deltaTime;
        g = new Vector3(0, 9.8f, 0);
        vy = new Vector3(0, 50, 0);
        vx = new Vector3(0, 0, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = (vy + vx) * step;
        transform.Translate(target);
        vy -= g * step;
        Debug.Log(transform.position.ToString());
    }
}
