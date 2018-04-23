using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class CollisionRev : MonoBehaviour {
    public UFOObject ufo;
    private void OnCollisionEnter(Collision collision)
    {/*
        Debug.Log("Collision @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + collision.gameObject.name);
        if (collision.collider) Debug.Log("Collider belongs to :" + collision.collider.gameObject.name);
        if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);*/

        UFOFactory factory = UFOFactory.getInstance();
        if(ufo!=null)//hit on the ground.
        {
            factory.recycle(ufo);
            FirstController firstController = Director.getInstance().currentSceneController as FirstController;
            firstController.ShotGround();
        }

    }
}
