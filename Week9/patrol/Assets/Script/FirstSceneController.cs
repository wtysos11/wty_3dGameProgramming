using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstSceneController : MonoBehaviour, ISceneController
{
    public GameObject player;
    public UserInterface userInterface;
    public readonly Vector3 originPos = new Vector3(50, 3, 50);
    public MonsterFactory factory;
    public MonsterActionManager actionManager;
    public EventManager eventManager;
    MonsterController[] monster;
    public int score = 0;
    Vector3[] monsterPosition;
    int monsterNumber = 15;
    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        factory = this.transform.gameObject.AddComponent<MonsterFactory>();
        actionManager = this.transform.gameObject.AddComponent<MonsterActionManager>();
        eventManager = this.transform.gameObject.AddComponent<EventManager>();
        userInterface = this.transform.gameObject.AddComponent<UserInterface>();
        makeMap();
        monsterPosition = new Vector3[monsterNumber];
        monsterPosition[0] = new Vector3(10, 0, 70);
        monsterPosition[1] = new Vector3(30, 0, 60);
        monsterPosition[2] = new Vector3(15, 0, 30);
        monsterPosition[3] = new Vector3(30, 0, 25);
        monsterPosition[4] = new Vector3(40, 0, 20);
        monsterPosition[5] = new Vector3(70, 0, 10);
        monsterPosition[6] = new Vector3(60, 0, 70);
        monsterPosition[7] = new Vector3(50, 0, 60);
        monsterPosition[8] = new Vector3(75, 0, 75);
        monsterPosition[9] = new Vector3(60, 0, 100);
        monsterPosition[10] = new Vector3(80, 0, 110);
        monsterPosition[11] = new Vector3(100, 0, 100);
        monsterPosition[12] = new Vector3(100, 0, 70);
        monsterPosition[13] = new Vector3(100, 0, 60);
        monsterPosition[14] = new Vector3(110, 0, 30);
        this.LoadResources();
    }

    //ISceneController接口类，负责加载资源
    public void LoadResources()
    {
        player = Object.Instantiate(Resources.Load("unitychan", typeof(GameObject))) as GameObject;
        player.transform.position = new Vector3(10, 10, 100);
        MonsterController[] monster = new MonsterController[monsterNumber];
        for (int i = 0; i < monsterNumber; i++)
        {
            monster[i] = factory.produceMonster();
            monster[i].transform.position = monsterPosition[i];
            actionManager.randomMove(monster[i]);
        }
    }

    private void makeMap()
    {
        float width = 1f;
        float long1 = 120f;
        float long2 = 100f;
        float height = 4f;
        float middleLong = 15f;
        float verticalLong = long1 / 3 * 2;
        GameObject originStone = Object.Instantiate(Resources.Load("wall", typeof(GameObject))) as GameObject;
        GameObject westWall = makeWall(originStone, new Vector3(width / 2, height / 2, long1 / 2), new Vector3(width, height, long1));
        GameObject eastWall = makeWall(originStone, new Vector3(long1 + 1 + width / 2, height / 2, long1 / 2), new Vector3(width, height, long1));
        GameObject southWall = makeWall(originStone, new Vector3(width + long2 / 2, height / 2, width / 2), new Vector3(long2, height, width));
        GameObject northWall = makeWall(originStone, new Vector3(width + long1 / 2, height / 2, width / 2 + long1 - 2), new Vector3(long1, height, width));
        GameObject Vertical1 = makeWall(originStone, new Vector3(long1 / 3 + width / 2, height / 2, long1 / 3 * 2), new Vector3(width, height, verticalLong));
        GameObject Vertical2 = makeWall(originStone, new Vector3(long1 / 3 * 2 + width / 2, height / 2, long1 / 3), new Vector3(width, height, verticalLong));
        GameObject middleNorthWestOne = makeWall(originStone, new Vector3(width / 2 + middleLong / 2, height / 2, long1 / 3 * 2), new Vector3(middleLong, height, width));
        GameObject middleSouthWestOne = makeWall(originStone, new Vector3(width / 2 + middleLong / 2, height / 2, long1 / 3), new Vector3(middleLong, height, width));
        GameObject middleSouthWestTwo = makeWall(originStone, new Vector3(long1 / 3, height / 2, width / 2 + middleLong / 2), new Vector3(width, height, middleLong));
        GameObject middleNorthEastOne = makeWall(originStone, new Vector3(long1 / 3*2, height / 2, width / 2 + middleLong / 2+long1/3*2), new Vector3(width, height, middleLong));
        GameObject middleCenterLeftDown = makeWall(originStone, new Vector3(width / 2 + middleLong / 2 + long1 / 3, height / 2, long1 / 3), new Vector3(middleLong, height, width));
        GameObject middleCenterRightUp = makeWall(originStone, new Vector3(-1*width / 2 - middleLong / 2 + long1 / 3*2, height / 2, long1 / 3 * 2), new Vector3(middleLong, height, width));
        GameObject middleRightDown = makeWall(originStone, new Vector3(width / 2 + middleLong / 2 + long1 / 3*2, height / 2, long1 / 3), new Vector3(middleLong, height, width));
        GameObject middleRightUp = makeWall(originStone, new Vector3(-1 * width / 2 - middleLong / 2 + long1, height / 2, long1 / 3 * 2), new Vector3(middleLong, height, width));
        GameObject middleLeftUp = makeWall(originStone, new Vector3(-1 * width / 2 - middleLong / 2 + long1/3, height / 2, long1 / 3 * 2), new Vector3(middleLong, height, width));
        originStone.SetActive(false);
    }

    private GameObject makeWall(GameObject origin,Vector3 position,Vector3 size)
    {
        GameObject target = GameObject.Instantiate(origin);
        target.transform.position = position;
        target.transform.localScale = size;
        return target;
    }

    public void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.ScoreChange += AddScore;
        EventManager.GameoverChange += Gameover;
        EventManager.GamewinChange += Gamewin;
    }

    private void OnDisable()
    {
        EventManager.ScoreChange -= AddScore;
        EventManager.GameoverChange -= Gameover;
        EventManager.GamewinChange -= Gamewin;
    }

    private void AddScore()
    {
        score++;
        Debug.Log("Escape successfully");
    }

    private void Gameover()
    {
        Debug.Log("Gameover, your score is " + score);
        Destroy(player);
        userInterface.status = 1;
    }

    private void Gamewin()
    {
        Debug.Log("Gamewin, your score is " + score);
        Destroy(player);
        userInterface.status = 1;
    }

    public void restart()
    {
        factory.clearAll();
        score = 0;
        userInterface.status = 0;
        this.LoadResources();
    }
}
