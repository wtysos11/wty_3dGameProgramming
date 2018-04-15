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
}
