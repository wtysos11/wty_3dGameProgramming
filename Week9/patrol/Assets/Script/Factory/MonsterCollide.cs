using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollide : MonoBehaviour {
    public MonsterController monster;
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision @" + Time.frameCount.ToString());
        //Debug.Log("GameObject is :" + collision.gameObject.name);//unitychan
        //if (collision.collider) Debug.Log("Collider belong to :" + collision.collider.gameObject.name);//unitychan
        //if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);//unitychan
        //Debug.Log(collision.transform.tag); //player
    }
}
