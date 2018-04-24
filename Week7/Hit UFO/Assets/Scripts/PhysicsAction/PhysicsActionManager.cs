using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsActionManager : MonoBehaviour {

    public void addForce(GameObject gameobject,Vector3 force)
    {
        ConstantForce originForce = gameobject.GetComponent<ConstantForce>();
        if(originForce!=null)
        {
            originForce.enabled = true;
            originForce.force = force;
        }
        else
        {
            gameobject.AddComponent<ConstantForce>().force = force;
        }
    }

    public void removeForce(GameObject gameobject)
    {
        if(gameObject.GetComponent<ConstantForce>())
             gameobject.GetComponent<ConstantForce>().enabled = false;
    }
}
