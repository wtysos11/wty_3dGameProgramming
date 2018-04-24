using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
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
