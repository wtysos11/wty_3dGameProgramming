using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    public GameObject gameObject;
    public Transform transform;
    public int id;
    public MonsterController(GameObject gb)
    {
        gameObject = gb;
        transform = gb.transform;
    }

    public void visible()
    {
        this.gameObject.SetActive(true);
    }
    public void invisible()
    {
        this.gameObject.SetActive(false);
    }
    public void setPosition(Vector3 origin)
    {
        this.transform.position = origin;
    }
}
