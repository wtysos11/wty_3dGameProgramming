﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTween;
public class test : MonoBehaviour {
    private void Awake()
    {
        transform.gameObject.AddComponent<TweenList>();
    }
    // Use this for initialization
    void Start () {
        transform.DoMove(new Vector3(5, 5, 5), 3.0f).DoMove(new Vector3(0,0,0),3.0f);
	}
}
