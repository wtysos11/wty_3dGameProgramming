using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
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

    /*
判断距离
如果超出指定距离，就自动把它撤销。
*/
    private void Update()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        Vector3 origin = firstController.originPos;
        float dist = Vector3.Distance(origin, ufoObj.ufo.transform.position);
        if(dist>40)
        {
            firstController.HitOnGround(ufoObj);
        }

    }
}
