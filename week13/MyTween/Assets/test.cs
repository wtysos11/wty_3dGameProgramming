using System.Collections;
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
        transform.DoMove(new Vector3(5, 5, 5), 3.0f)
            .DoMove(new Vector3(0,0,0),3.0f)
            .DoScale(new Vector3(5,5,5),5.0f)
            .DoDelay(2)
            .OnComplete((tween) =>
            {
                tween.transform.DoMove(new Vector3(5, 0, 0), 3.0f);
            })
            .OnComplete((tween) =>
            {
                tween.transform.DoScale(new Vector3(0.5f, 0.5f, 0.5f), 3.0f);
            });
	}
}
