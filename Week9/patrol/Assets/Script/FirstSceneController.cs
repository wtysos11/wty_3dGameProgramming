using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstSceneController : MonoBehaviour, ISceneController
{
    public UserInterface userInterface;
    public readonly Vector3 originPos = new Vector3(50, 3, 50);
    public MonsterFactory factory;
    public MonsterActionManager actionManager;
    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        factory = this.transform.gameObject.AddComponent<MonsterFactory>();
        actionManager = this.transform.gameObject.AddComponent<MonsterActionManager>();
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
        MonsterController monster = factory.produceMonster();
        monster.transform.position = new Vector3(24, 0, 24);
        actionManager.randomMove(monster);

    }
    public void Start()
    {

    }



}
