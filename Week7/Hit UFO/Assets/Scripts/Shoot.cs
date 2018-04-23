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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (firstController.userInterface.status == 0)
                firstController.userInterface.status = 1;
            else if (firstController.userInterface.status == 1)
                firstController.userInterface.status = 0;
        }

        if (firstController.userInterface.status == 1)
            return;

        //鼠标左键
        if(Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("input mouse position:" + Input.mousePosition.ToString());
            bool shootground=false;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            for(int i=0;i<hits.Length;i++)
            {
                RaycastHit hit = hits[i];

                if(hit.transform.name=="Terrain")
                {
                    shootground = true;//because of unknown reason,sometimes shoot on the ufo will first shoot on the ground(situation:ufo flies very low and in the grass)
                    continue;
                }
                
                UFOObject ufoObject = hit.transform.GetComponent<UFORender>().ufoObj;
                if(ufoObject!=null)
                {
                    firstController.UFOIsShot(ufoObject);
                    return;
                }
            }
            //没有打中飞碟，扣分
            firstController.ShotGround();
        }

    }
}
