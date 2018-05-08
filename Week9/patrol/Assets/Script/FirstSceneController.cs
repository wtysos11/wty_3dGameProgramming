using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController
{
    public UserInterface userInterface;
    public readonly Vector3 originPos = new Vector3(50, 3, 50);
    public MonsterFactory factory;
    public MonsterActionManager acitonManager;
    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        factory = Singleton<MonsterFactory>.Instance;
        acitonManager = Singleton<MonsterActionManager>.Instance;
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
        MonsterController monster = factory.produceMonster();
        monster.transform.position = new Vector3(24, 0, 24);
        acitonManager.randomMove(monster);

    }
    public void Start()
    {

    }



}
