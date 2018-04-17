# 3d Programming
使用分支管理来存储各周的文件，master只保留Readme
[演示视频请点此处](https://www.bilibili.com/video/av22200145/)

# 第六周 打飞碟
参考代码：https://blog.csdn.net/c486c/article/details/79952255
具体代码：https://github.com/wtysos11/wty_3dGameProgramming/tree/Week6
## 程序基本框架
首先是沿用了上周所做的动作管理器与场景控制器的框架，然后使用单例工厂来对飞碟进行生产、回收来减少消耗。

## 规则设定
设定每一阶段打中飞碟的分数为：阶段数+1，并且规定没有打中飞碟分数减一。实现了ESC菜单，但是还没做出暂停。

## 具体代码
### 动作管理
使用特化的UFOManager类进行管理。
飞碟动作产生，产生多个随机位置，并最后让它们回到原点：
```C#
    public void ufoRandomMove(UFOObject ufo)
    {
        float moveSpeed = ufo.attr.speed;
        Vector3 currentPos = ufo.attr.originPosition;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y + 10),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y + 5),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y + 5),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(ufo.ufo.transform.position.x, ufo.ufo.transform.position.y, ufo.ufo.transform.position.z);

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置4
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3,moveAction4 });//制作SequenceAction
        addAction(ufo.ufo, sequenceAction, this);
    }

```
### UFO管理
使用带缓存的工厂模式对UFO进行管理，由工厂统一创建和回收UFO。
```C#
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
}

```

### 场景控制器
场景控制器与导演类与上次作业基本相同，飞碟击中与没有击中的判断和每个回合的判断都有场景控制器来决定。第一人称控制器也由场景控制器负责初始化。
```C#

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
        ufoFactory = UFOFactory.getInstance();
        shoot = gameObject.AddComponent<Shoot>() as Shoot;
        difficultyManager = new DifficultyManager();
        score = new Score();
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
```

### 射击对象
主要设计部分的控制对象。
```C#
public class Shoot : MonoBehaviour {
    public Camera camera;
    private FirstController firstController;

    private void Start()
    {
        camera = Camera.main;
        firstController = Director.getInstance().currentSceneController as FirstController;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (firstController.userInterface.status == 0)
                firstController.userInterface.status = 1;
            else if (firstController.userInterface.status == 1)
                firstController.userInterface.status = 0;
        }

        if (firstController.userInterface.status == 1)
            return;

        //鼠标左键
        if(Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("input mouse position:" + Input.mousePosition.ToString());
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            //Debug.Log("hits.Length" + hits.Length.ToString());
            for(int i=0;i<hits.Length;i++)
            {
                RaycastHit hit = hits[i];
                if(hit.transform.name=="Terrain")
                {
                    firstController.ShotGround();
                    return;
                }
                UFOObject ufoObject = hit.transform.GetComponent<UFORender>().ufoObj;
                if(ufoObject!=null)
                {
                    firstController.UFOIsShot(ufoObject);
                    return;
                }
            }
            //没有打中飞碟，扣分
            firstController.ShotGround();
        }

    }
}

```