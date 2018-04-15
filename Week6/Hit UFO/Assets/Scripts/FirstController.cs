using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController
{
    UserInterface userInterface;
    public UFOActionManager actionManager;
    UFOFactory ufoFactory;

    bool roundStarted = false;

    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userInterface = gameObject.AddComponent<UserInterface>() as UserInterface;
        actionManager = gameObject.AddComponent<UFOActionManager>() as UFOActionManager;
        ufoFactory = gameObject.AddComponent<UFOFactory>() as UFOFactory;
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
        new FirstCharacterController();
        Instantiate(Resources.Load("Terrain"));
    }
    public void Start()
    {
        newRound();
    }

    //restart Button，可以后期再做
    public void restart()
    {

    }
    private void newRound()
    {
        roundStarted = true;
        UFOObject[] ufoObjects = new UFOObject[10];
        for(int i=0;i<10;i++)
        {
            ufoObjects[i] = ufoFactory.produceUFO(new UFOAttr(1f, 1f, new Vector3(50, 5, 50)));
            actionManager.ufoRandomMove(ufoObjects[i]);
        }
        
    }
    //某个UFO对象被击中了
    public void UFOIsShot(UFOObject ufoObject)
    {
        actionManager.removeAction(ufoObject.ufo);
        ufoFactory.recycle(ufoObject);
    }
}
