using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollide : MonoBehaviour {
    public MonsterController monster;
    public MonsterChaser chaser;
    private void OnCollisionEnter(Collision collision)//碰撞，装上玩家的时候游戏结束
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.activeSelf)
        {
            Debug.Log("In monstercollide, player gameover");
            Singleton<EventManager>.Instance.PlayerGameover();
        }
        /*
        Debug.Log("Collision @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + collision.gameObject.name);//unitychan
        if (collision.collider) Debug.Log("Collider belong to :" + collision.collider.gameObject.name);//unitychan
        if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);//unitychan
        Debug.Log(collision.transform.tag); //player*/
    }
    private void OnTriggerEnter(Collider other)//开始追逐
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
    private void OnTriggerExit(Collider other)//结束追逐
    {
        FirstSceneController firstScene = Mygame.Director.getInstance().currentSceneController as FirstSceneController;
        MonsterActionManager actionManager = firstScene.GetComponent<MonsterActionManager>();
        chaser.disable();
        actionManager.randomMove(monster);
        Singleton<EventManager>.Instance.PlayerEscape();
        Debug.Log("In monstercollide, player escape");
        /*
        Debug.Log("Trigger exit @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + other.gameObject.name);//unitychan
        Debug.Log(other.transform.tag); //player*/
    }
}
