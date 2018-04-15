using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class Shoot : MonoBehaviour {
    public Camera camera;
    private FirstController firstController;
    LayerMask layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Shootable", "RayFinish");
    }
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
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit, Mathf.Infinity,layerMask))
            {
                if(hit.transform.gameObject.layer == 8)
                {
                    UFOObject ufoObject = hit.transform.GetComponent<UFORender>().ufoObj;
                    firstController.UFOIsShot(ufoObject);
                }

            }
        }
    }
}
