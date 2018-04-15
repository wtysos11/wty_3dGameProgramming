using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UFOFactory : MonoBehaviour {
    Queue<UFOObject> freeQueue; //储存正在空闲时的UFO
    List<UFOObject> usingList;  //储存正在使用时的UFO
    private int totalNumber = 0;

    GameObject originalUFO;//UFO原型
    

    private void Awake()
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
            totalNumber++;
        }
        else
        {
            newUFO = freeQueue.Dequeue();
        }

        newUFO.setAttr(attr);
        usingList.Add(newUFO);
        newUFO.visible();
        return newUFO;
    }

    public void recycle(UFOObject ufo)
    {
        ufo.invisible();
        usingList.Remove(ufo);
        freeQueue.Enqueue(ufo);
    }
}
