using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UFOFactory : MonoBehaviour {
    Queue<UFOObject> freeQueue; //储存正在空闲时的UFO
    List<UFOObject> usingList;  //储存正在使用时的UFO
    private int totalNumber = 0;

    GameObject originalUFO;//UFO原型

    private static UFOFactory _instance;
    public static UFOFactory getInstance()
    {
        if(_instance == null)
        {
            _instance = new UFOFactory();

        }
        return _instance;
    }
    protected UFOFactory()
    {
        freeQueue = new Queue<UFOObject>();
        usingList = new List<UFOObject>();

        originalUFO = Object.Instantiate(Resources.Load("ufo", typeof(GameObject))) as GameObject;
        
        originalUFO.SetActive(false);
    }

    public UFOObject produceUFO(UFOAttr attr)
    {
        UFOObject newUFO;
        if(freeQueue.Count == 0)
        {
            GameObject newObj = GameObject.Instantiate(originalUFO);
            newUFO = new UFOObject(newObj);
            newUFO.ufo.transform.name = "ufo" + totalNumber;
            CollisionRev rev = newUFO.ufo.AddComponent<CollisionRev>();
            rev.ufo = newUFO;
            totalNumber++;
        }
        else
        {
            newUFO = freeQueue.Dequeue();
        }

        newUFO.setAttr(attr);
        usingList.Add(newUFO);
        newUFO.randomChange();
        newUFO.visible();
        return newUFO;
    }

    public void recycle(UFOObject ufo)
    {
        ufo.invisible();
        usingList.Remove(ufo);
        freeQueue.Enqueue(ufo);
    }

    public bool usingListEmpty()
    {
        if (usingList.Count == 0)
            return true;
        else
            return false;
    }
    public List<UFOObject> getUsingList()
    {
        return usingList;
    }
}
