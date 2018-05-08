using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MonsterChaser : MonoBehaviour {
    bool status=false;
    private GameObject player;
    float chaseSpeed = 5f;
    public void enable(GameObject aim)
    {
        player = aim;
        status = true;
    }
    public void disable()
    {
        player = null;
        status = false;
    }
    private void Update()
    {
        if(status&&player!=null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            changeAngle();
        }
    }
    private void changeAngle()
    {
        Vector3 origin = this.transform.position;
        Vector3 target = player.transform.position;
        double deltaX = target.x - origin.x;
        double deltaZ = target.z - origin.z;
        double tan = deltaX / deltaZ;
        double arcTan = Math.Atan(tan);
        double degree = 180 / Math.PI * arcTan;
        if (degree <= 0)
        {
            degree += 180;
        }
        if (target.x < origin.x)
        {
            degree += 180;
        }
        this.transform.rotation = Quaternion.Euler(0, (float)degree, 0);
    }
}
