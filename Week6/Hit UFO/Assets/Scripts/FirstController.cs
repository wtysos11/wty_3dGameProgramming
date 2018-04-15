using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController
{
    public UserInterface userInterface;
    public UFOActionManager actionManager;
    UFOFactory ufoFactory;
    Shoot shoot;
    bool roundStarted = false;
    public Score score;
    public DifficultyManager difficultyManager;
    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userInterface = gameObject.AddComponent<UserInterface>() as UserInterface;
        actionManager = gameObject.AddComponent<UFOActionManager>() as UFOActionManager;
        ufoFactory = gameObject.AddComponent<UFOFactory>() as UFOFactory;
        shoot = gameObject.AddComponent<Shoot>() as Shoot;
        score = new Score();
        difficultyManager = new DifficultyManager();
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
        new FirstCharacterController();
        GameObject terrain=GameObject.Instantiate(Resources.Load("Terrain")) as GameObject;
        terrain.name = "Terrain";
    }
    public void Start()
    {
        newRound();
    }

    //restart Button，可以后期再做
    public void restart()
    {
        List<UFOObject> list = ufoFactory.getUsingList();
        List<UFOObject> usingList = new List<UFOObject>();
        list.ForEach(i => usingList.Add(i));
        int ceil = usingList.Count;
        for(int i=0;i<ceil;i++)
        {
            Debug.Log(i + " " + ceil+" "+usingList.Count);
            UFOObject ufoObj = usingList[i];
            ufoFactory.recycle(ufoObj);
            actionManager.removeAction(ufoObj.ufo);
        }
        difficultyManager.clear();
        score.clear();
        newRound();

    }
    private void newRound()
    {
        roundStarted = true;
        UFOObject[] ufoObjects = new UFOObject[10];
        for(int i=0;i<10;i++)
        {
            ufoObjects[i] = ufoFactory.produceUFO(difficultyManager.getAttr());
            actionManager.ufoRandomMove(ufoObjects[i]);
        }
        
    }
    private void roundDone()
    {
        roundStarted = false;
        difficultyManager.levelUp();
        newRound();
    }

    //某个UFO对象被击中了
    public void UFOIsShot(UFOObject ufoObject)
    {
        actionManager.removeAction(ufoObject.ufo);
        ufoFactory.recycle(ufoObject);
        score.update();

        if(ufoFactory.usingListEmpty())
        {
            this.roundDone();
        }

    }
    public void ShotGround()
    {
        score.fail();
    }
}
