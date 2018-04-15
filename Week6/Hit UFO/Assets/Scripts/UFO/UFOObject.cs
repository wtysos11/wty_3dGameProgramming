using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOObject{
    readonly public GameObject ufo;
    readonly public int id;
    static private int _count=0;
    public UFOAttr attr;
    private UFORender render;

    public UFOObject()
    {
        id = _count;
        _count++;
    }
    public UFOObject(GameObject gameobject)
    {
        this.ufo = gameobject;
        render = gameobject.AddComponent<UFORender>();
        render.ufoObj = this;

        id = _count;
        _count++;
    }

    public void setAttr(UFOAttr attr)
    {
        this.attr = attr;
        this.ufo.transform.position = attr.originPosition;
    }

    public void visible()
    {
        this.ufo.SetActive(true);
    }

    public void invisible()
    {
        this.ufo.SetActive(false);
    }
    public void randomChange()
    {
        Vector3 origin = this.ufo.transform.position;
        this.ufo.transform.position = new Vector3(
            Random.Range(origin.x - 2, origin.x + 2),
            Random.Range(1, origin.y + 5),
            Random.Range(origin.z - 10, origin.z + 10)
            );
    }
}
