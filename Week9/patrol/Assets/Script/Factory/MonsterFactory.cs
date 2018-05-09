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
        originalMonster.transform.position = new Vector3(25, 0, 25);
        originalMonster.SetActive(false);
    }

    public MonsterController produceMonster()
    {
        MonsterController monster;
        if (freeQueue.Count == 0)
        {
            GameObject newObj = GameObject.Instantiate(originalMonster);
            monster = new MonsterController(newObj);
            monster.transform.name = "monster" + totalNumber;
            monster.id = totalNumber;
            MonsterChaser chasing = monster.gameObject.AddComponent<MonsterChaser>();//追逐器
            MonsterCollide rev = monster.gameObject.AddComponent<MonsterCollide>();//碰撞处理器
            rev.monster = monster;
            rev.chaser = chasing;
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

    public void clearAll()
    {
        var stack = new Stack<MonsterController>(usingList);
        while(stack.Count!=0)
        {
            MonsterController monster = stack.Pop();
            this.recycle(monster);
        }
    }
    
}
