using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollide : MonoBehaviour {
    public MonsterController monster;
    public MonsterChaser chaser;
    private void OnCollisionEnter(Collision collision)
    {
        /*
        Debug.Log("Collision @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + collision.gameObject.name);//unitychan
        if (collision.collider) Debug.Log("Collider belong to :" + collision.collider.gameObject.name);//unitychan
        if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);//unitychan
        Debug.Log(collision.transform.tag); //player*/
    }
    private void OnTriggerEnter(Collider other)
    {
        FirstSceneController firstScene = Mygame.Director.getInstance().currentSceneController as FirstSceneController;
        MonsterActionManager actionManager = firstScene.GetComponent<MonsterActionManager>();
        chaser.enable(other.gameObject);
        actionManager.removeAction(monster.gameObject);
        /*
        Debug.Log("Trigger enter @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + other.gameObject.name);//unitychan
        Debug.Log(other.transform.tag); */
    }
    private void OnTriggerExit(Collider other)
    {
        FirstSceneController firstScene = Mygame.Director.getInstance().currentSceneController as FirstSceneController;
        MonsterActionManager actionManager = firstScene.GetComponent<MonsterActionManager>();
        chaser.disable();
        actionManager.randomMove(monster);
        /*
        Debug.Log("Trigger exit @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + other.gameObject.name);//unitychan
        Debug.Log(other.transform.tag); //player*/
    }
}
