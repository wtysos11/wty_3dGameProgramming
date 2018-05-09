# 第九周作业：巡逻兵
## 参考资料
主体架构和素材参考了这篇文章：[文章](http://www.chenxd59.cn/?p=201)。不得不说，Unitychan是真的挺不错的，如果下次有机会我还是会用这个模型。
## 规则说明与演示
使用鼠标或键盘操控角色，摆脱追击的巡逻兵兽人，从天空之城中找到道路逃出生天。（被抓到会发生不可描述的事情）
每摆脱一次兽人会加一分。
演示视频：[视频](https://www.bilibili.com/video/av23200199/)

## 具体实现与代码
架构与上面文章的基本相似（其实只是没时间画UML图），详细代码在这里：[代码](https://github.com/wtysos11/wty_3dGameProgramming/tree/Week9)
由于上面那篇按照结构讲的挺好的，我就照自己的实现步骤讲一下代码
实现步骤：
 1. 角色加载和动画载入
 2. 第一人称控制和相应的动画变化
 3. 动作类和动作管理器的具体实现
 4. 巡逻兵基本动作的实现
 5. 巡逻兵碰撞实现
 6. 巡逻兵追逐实现
 7. 消息发布订阅管理器实现
 8. 游戏界面实现
 9. 游戏结束、胜利实现
 10. 地图制作

### 角色加载和动画载入
主要是Unitychan的载入，由于制作者提供了模型，实际上就是把模型拖出去修改了一下，然后将Prefab放入Resources文件夹后载入即可。具体实现在`FirstSceneController.cs`文件中的`public void LoadResources()`函数
```C#
        player = Object.Instantiate(Resources.Load("unitychan", typeof(GameObject))) as GameObject;
        player.transform.position = new Vector3(10, 10, 100);
```
动画主要用到的函数是animator的API，像是SetFloat或是SetTrigger都是可能用到的。
由于制作者提供了API，所以只要修改一下就好了，具体实现在后面的控制实现。

### 第一人称控制和相应的动画变化
 * 控制主要用的是虚拟轴和鼠标进行控制。
 * 虚拟轴的按键可以在Unity3d中的Edit->Project Settings->Input中进行修改。
 * 第一人称的视角实现实际上就是将摄像机绑到角色上。
 * 第一人称鼠标控制是网上找的[代码](https://blog.csdn.net/chenggong2dm/article/details/35988563)，我修改了一下然后直接绑了上去，效果还不错。
 具体控制实现(FirstCharacterController.cs)
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCharacterController : MonoBehaviour {
    public float rotateSpeed = 16.0F;
    public float runningSpeed = 10.0F;//running speed
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(this.transform.position.y<-10)
        {
            Singleton<EventManager>.Instance.PlayerWin();
        }

        float running = Input.GetAxis("Vertical") * runningSpeed;
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed;
        if (running != 0 || rotate != 0)
        {
            animator.ResetTrigger("Rest");
        }
        else
        {
            animator.SetTrigger("Rest");
            return;
        }
        running *= Time.deltaTime;
        rotate *= Time.deltaTime;
        rotate *= 20;
        //Debug.Log("rotate:"+rotate);
        transform.Translate(0, 0, running);
        transform.Rotate(0, rotate, 0);
        animator.SetFloat("Speed", running);
        
    }



}

```

### 动作管理器的具体实现
因为之前已经实现了BaseActionManager在内的一系列动作实现类，所以只要在其基础上继承修改就好了：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class MonsterActionManager : BaseActionManager,ActionCallback
{
    private static int MAX_RANGE = 1;
    public void randomMove(MonsterController monster)
    {
        float moveSpeed = 1;
        Vector3 currentPos = monster.transform.position;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget3, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget4, moveSpeed);//前往位置4

        Vector3 randomTarget5 = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z);
        LineAction moveAction5 = LineAction.GetBaseAction(randomTarget5, moveSpeed);//前往位置5
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3, moveAction4,moveAction5 });//制作SequenceAction
        addAction(monster.gameObject, sequenceAction, this);
    }
    public void anotherRandomMove(MonsterController monster)
    {
        this.removeAction(monster.gameObject);
        float moveSpeed = 1;
        Vector3 currentPos = monster.transform.position;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget3, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget4, moveSpeed);//前往位置4

        Vector3 randomTarget5 = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z);
        LineAction moveAction5 = LineAction.GetBaseAction(randomTarget5, moveSpeed);//前往位置5
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3, moveAction4, moveAction5 });//制作SequenceAction
        addAction(monster.gameObject, sequenceAction, this);
    }

    public void actionDone(BaseAction source)
    {

    }


}

```

### 巡逻兵基本动作实现
巡逻兵的基本动作就是修改巡逻兵的动画，不好放出来，就不放了。

### 巡逻兵碰撞与追逐的实现
就是给巡逻兵、角色等套上collider，给角色套上刚体。
因为巡逻兵是判断的主体，所以判断脚本挂在巡逻兵身上。
追逐也由巡逻兵判断，就像前文所说，给巡逻兵设置一个大一点的碰撞体，然后勾选isTrigger，使用OnTriggerEnter和OnTriggerExit来实现。同学提醒我这个可能有问题，因为你如果卡在边界上会大量触发ScoreChange事件，这个可以用FixedUpdate来计算时间，维护一个标志变量，这个标志变量变化后一秒钟内不会发生改变。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollide : MonoBehaviour {
    public MonsterController monster;
    public MonsterChaser chaser;
    private void OnCollisionEnter(Collision collision)//碰撞，装上玩家的时候游戏结束
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.activeSelf)
        {
            Debug.Log("In monstercollide, player gameover");
            Singleton<EventManager>.Instance.PlayerGameover();
        }
        /*
        Debug.Log("Collision @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + collision.gameObject.name);//unitychan
        if (collision.collider) Debug.Log("Collider belong to :" + collision.collider.gameObject.name);//unitychan
        if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);//unitychan
        Debug.Log(collision.transform.tag); //player*/
    }
    private void OnTriggerEnter(Collider other)//开始追逐
    {
        FirstSceneController firstScene = Mygame.Director.getInstance().currentSceneController as FirstSceneController;
        MonsterActionManager actionManager = firstScene.GetComponent<MonsterActionManager>();
        chaser.enable(other.gameObject);
        actionManager.removeAction(monster.gameObject);
        /*
        Debug.Log("Trigger enter @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + other.gameObject.name);//unitychan
        Debug.Log(other.transform.tag); */
    }
    private void OnTriggerExit(Collider other)//结束追逐
    {
        FirstSceneController firstScene = Mygame.Director.getInstance().currentSceneController as FirstSceneController;
        MonsterActionManager actionManager = firstScene.GetComponent<MonsterActionManager>();
        chaser.disable();
        actionManager.randomMove(monster);
        Singleton<EventManager>.Instance.PlayerEscape();
        Debug.Log("In monstercollide, player escape");
        /*
        Debug.Log("Trigger exit @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + other.gameObject.name);//unitychan
        Debug.Log(other.transform.tag); //player*/
    }
}

```
### 消息发布订阅管理器实现
消息发布器：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;

    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;

    public delegate void GameWin();
    public static event GameWin GamewinChange;

    public void PlayerEscape()
    {
        if (ScoreChange != null)
        {
            ScoreChange();
        }
    }

    public void PlayerGameover()
    {
        if(GameoverChange!=null)
        {
            GameoverChange();
        }
    }

    public void PlayerWin()
    {
        if (GamewinChange != null)
        {
            GamewinChange();
        }
    }
}

```

### 游戏界面实现
UserInterface.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UserInterface : MonoBehaviour
{
    FirstSceneController firstController;
    GUIStyle style;
    GUIStyle buttonStyle;
    public int status = 0;
    private void Start()
    {
        this.firstController = Director.getInstance().currentSceneController as FirstSceneController;

        style = new GUIStyle();
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
    }

    private void OnGUI()
    {
        if(status == 1)
        {
            string ans = "game is over, your score is " + firstController.score;
            GUI.Label(new Rect(Screen.width / 2 - 50, 20, 70, 70), ans, style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50 , Screen.height / 2 - 180, 140, 70), "Restart", buttonStyle))
            {
                firstController.restart();
                status = 0;
            }
        }
    }
}

```
### 游戏结束、胜利实现
具体实现(FirstSceneController.cs)：
```C#
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
```
### 地图制作
```C#
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
```