using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAdapter : ActionAdapterInterface {

    private bool mode;//mode = false 时使用运动学，mode=true时使用重力
    public UFOActionManager actionManager;
    public PhysicsActionManager physicsActionManager;
    public ActionAdapter(GameObject gameObject)
    {
        mode = false;
        actionManager = gameObject.AddComponent<UFOActionManager>() as UFOActionManager;
        physicsActionManager = gameObject.AddComponent<PhysicsActionManager>() as PhysicsActionManager;
    }

    public void switchMode()
    {
        mode = !mode;
    }

    public bool getMode()
    {
        return mode;
    }


    public void ufoRandomMove(UFOObject ufoObject)
    {
        if(mode == false)
        {
            actionManager.ufoRandomMove(ufoObject);
            physicsActionManager.removeForce(ufoObject.ufo);
        }
        else
        {
            actionManager.removeAction(ufoObject.ufo);
            physicsActionManager.addForce(ufoObject.ufo, new Vector3(Random.Range(-1*ufoObject.attr.speed,ufoObject.attr.speed),
                Random.Range(-1*ufoObject.attr.speed,0),
                Random.Range(-1 * ufoObject.attr.speed, ufoObject.attr.speed)));
        }
    }

    public void removeAction(GameObject gameObject)
    {
        if(mode == false)
        {
            actionManager.removeAction(gameObject);
        }
        else
        {
            physicsActionManager.removeForce(gameObject);
        }
    }
}
