using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFORender : MonoBehaviour {

    public UFOObject ufoObj;
    private void Start()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            GameObject g = transform.GetChild(i).gameObject;
            g.AddComponent<UFORender>().ufoObj = ufoObj;
        }
    }
}
