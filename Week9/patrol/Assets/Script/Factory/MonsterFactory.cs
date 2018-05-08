using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class MonsterFactory : MonoBehaviour {
    Queue<MonsterController> freeQueue; //储存正在空闲时的UFO
    List<MonsterController> usingList;  //储存正在使用时的UFO
    private int totalNumber = 0;

    GameObject originalMonster;//UFO原型

    public MonsterFactory()
    {
        freeQueue = new Queue<MonsterController>();
        usingList = new List<MonsterController>();
    }

    private void Awake()
    {
        originalMonster = Object.Instantiate(Resources.Load("Monster", typeof(GameObject))) as GameObject;

        originalMonster.SetActive(false);
    }

    public MonsterController produceMonster()
    {
        MonsterController monster;
        if (freeQueue.Count == 0)
        {
            GameObject newObj = GameObject.Instantiate(originalMonster);
            monster = new MonsterController(newObj);
            monster.transform.name = "ufo" + totalNumber;
            monster.tag = "monster";
            monster.id = totalNumber;
            MonsterCollide rev = monster.gameObject.AddComponent<MonsterCollide>();
            rev.monster = monster;
            totalNumber++;
        }
        else
        {
            monster = freeQueue.Dequeue();
        }
        //Debug.Log("produce ufo's name:"+newUFO.ufo.transform.name);
        usingList.Add(monster);
        monster.visible();
        return monster;
    }

    public void recycle(MonsterController monster)
    {
        monster.invisible();
        usingList.Remove(monster);
        freeQueue.Enqueue(monster);
    }

    public bool usingListEmpty()
    {
        if (usingList.Count == 0)
            return true;
        else
            return false;
    }
    public List<MonsterController> getUsingList()
    {
        return usingList;
    }
    
}
