# 3d Programming
使用分支管理来存储各周的文件，master只保留Readme
[演示视频请点此处](https://www.bilibili.com/video/av22200145/)

# 第七周 打飞碟
## 程序基本框架
使用了一个动作适配器来在运动学动作和物理动作管理器之间进行切换

## 规则设定
设定每一阶段打中飞碟的分数为：阶段数+1，并且规定没有打中飞碟分数减一。实现了ESC菜单，但是还没做出暂停。
同时如果飞碟撞到地上或是飞离原点40个单位会自动消失，并且扣1分。如果分数为负的，则等级会下降。

## 具体代码
物理类
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsActionManager : MonoBehaviour {

    public void addForce(GameObject gameobject,Vector3 force)
    {
        ConstantForce originForce = gameobject.GetComponent<ConstantForce>();
        if(originForce!=null)
        {
            originForce.enabled = true;
            originForce.force = force;
        }
        else
        {
            gameobject.AddComponent<ConstantForce>().force = force;
        }
    }

    public void removeForce(GameObject gameobject)
    {
        if(gameObject.GetComponent<ConstantForce>())
             gameobject.GetComponent<ConstantForce>().enabled = false;
    }
}


```
### 适配器
接口与实现
```C#
public interface ActionAdapterInterface
{
    void ufoRandomMove(UFOObject ufoObject);
}

public class ActionAdapter : ActionAdapterInterface {

    private bool mode;//mode = false 时使用运动学，mode=true时使用重力
    public UFOActionManager actionManager;
    public PhysicsActionManager physicsActionManager;
    public ActionAdapter(GameObject gameObject)
    {
        mode = false;
        actionManager = gameObject.AddComponent<UFOActionManager>() as UFOActionManager;
        physicsActionManager = gameObject.AddComponent<PhysicsActionManager>() as PhysicsActionManager;
    }

    public void switchMode()
    {
        mode = !mode;
    }

    public bool getMode()
    {
        return mode;
    }


    public void ufoRandomMove(UFOObject ufoObject)
    {
        if(mode == false)
        {
            actionManager.ufoRandomMove(ufoObject);
            physicsActionManager.removeForce(ufoObject.ufo);
        }
        else
        {
            actionManager.removeAction(ufoObject.ufo);
            physicsActionManager.addForce(ufoObject.ufo, new Vector3(Random.Range(-1*ufoObject.attr.speed,ufoObject.attr.speed),
                Random.Range(-1*ufoObject.attr.speed,0),
                Random.Range(-1 * ufoObject.attr.speed, ufoObject.attr.speed)));
        }
    }

    public void removeAction(GameObject gameObject)
    {
        if(mode == false)
        {
            actionManager.removeAction(gameObject);
        }
        else
        {
            physicsActionManager.removeForce(gameObject);
        }
    }
}

```

### 撞击控制
使用了刚体组件后会有飞碟自己相撞的情况，需要与其和地面相撞的情况分开处理
```C#
public class CollisionRev : MonoBehaviour {
    public UFOObject ufo;
    private Dictionary<int, UFOObject> collisionCache;
    private readonly UFOObject TerrainEqual;//use to stand for terrain in ufoObject in collisionCache. Avoid mix with null.
    public CollisionRev()
    {
        collisionCache = new Dictionary<int, UFOObject>();
    }
    /*
     避免同tag相撞的想法：
        使用collisionCache，以Time.frameCount为索引来进行搜索。如果在collisionCache中没有索引，则将UFOObject对象加入其中。
        如果在collisionCache中有索引，进行判断
        使用Dictionary.ContainsKey(key)进行判断
            如果说两者都是UFO，那么忽略
            如果一者是UFO，一者是terrain，那么进行销毁。

       特殊情况：Terrain的撞击只会发生一次
         */
    private void OnCollisionEnter(Collision collision)
    {
        /*
        Debug.Log("Collision @" + Time.frameCount.ToString());
        Debug.Log("GameObject is :" + collision.gameObject.name);
        if (collision.collider) Debug.Log("Collider belongs to :" + collision.collider.gameObject.name);
        if (collision.rigidbody) Debug.Log("Rigidbody belong to :" + collision.rigidbody.gameObject.name);*/
        //判断在该帧之前是否有发生过判断
        //如果没有，直接插入，过
        //Debug.Log(collisionCache.ContainsKey(Time.frameCount));
        if(!collisionCache.ContainsKey(Time.frameCount))
        {
            if(collision.gameObject.name == "Terrain")
            {
                if (ufo != null)//hit on the ground.
                {
                    FirstController firstController = Director.getInstance().currentSceneController as FirstController;
                    firstController.HitOnGround(ufo);
                }
            }
            else
            {
                collisionCache[Time.frameCount] = ufo;
            }
        }
            //如果有，判断是否与地面发生撞击
        else if (collisionCache[Time.frameCount] == null || collision.gameObject.name == "Terrain")//有且只有一个Terrain
        {
            FirstController firstController = Director.getInstance().currentSceneController as FirstController;
            if (ufo != null)//hit on the ground.
            {
                firstController.HitOnGround(ufo);
            }
            else
            {
                firstController.HitOnGround(collisionCache[Time.frameCount]);
            }
        }
    }
}

```

### 位置控制
因为有可能会飞离太远导致基本不可能射中，所以设置飞离玩家所在点40个单位后自动销毁，销毁由FirstController控制。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UFORender : MonoBehaviour {

    public UFOObject ufoObj;
    private void Start()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            GameObject g = transform.GetChild(i).gameObject;
            g.AddComponent<UFORender>().ufoObj = ufoObj;
        }
    }

    /*
判断距离
如果超出指定距离，就自动把它撤销。
*/
    private void Update()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        Vector3 origin = firstController.originPos;
        float dist = Vector3.Distance(origin, ufoObj.ufo.transform.position);
        if(dist>40)
        {
            firstController.HitOnGround(ufoObj);
        }

    }
}

```