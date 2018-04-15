using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class Shoot : MonoBehaviour {
    public Camera camera;
    private FirstController firstController;

    private void Start()
    {
        camera = Camera.main;
        firstController = Director.getInstance().currentSceneController as FirstController;
    }
    private void Update()
    {
        //鼠标左键
        if(Input.GetButton("Fire1"))
        {
            //Debug.Log("input mouse position:" + Input.mousePosition.ToString());
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            //Debug.Log("hits.Length" + hits.Length.ToString());
            for(int i=0;i<hits.Length;i++)
            {
                RaycastHit hit = hits[i];
                UFOObject ufoObject = hit.transform.GetComponent<UFORender>().ufoObj;
                if(ufoObject!=null)
                {
                    firstController.UFOIsShot(ufoObject);
                    return;
                }
            }
        }
    }
}
