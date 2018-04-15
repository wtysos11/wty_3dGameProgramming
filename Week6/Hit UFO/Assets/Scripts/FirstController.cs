using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController
{
    UserInterface userInterface;
    public FirstSceneActionManager actionManager;

    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userInterface = gameObject.AddComponent<UserInterface>() as UserInterface;
        actionManager = gameObject.AddComponent<FirstSceneActionManager>() as FirstSceneActionManager;
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
    }

    //restart Button，可以后期再做
    public void restart()
    {

    }
}
