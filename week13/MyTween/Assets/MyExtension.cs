using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyTween
{
    public static class MyExtension
    {
        //具体的实现
        public static IEnumerator _DoMove(this MonoBehaviour mono,MyTween tween)
        {
            Debug.Log("enter implement _DoMove");
            Vector3 speed = (tween.target - tween.transform.position) / tween.duration;
            for(float f = tween.duration;f>=0.0f;f-=Time.deltaTime)
            {
                tween.transform.Translate(speed * Time.deltaTime);
                yield return null;
                while(tween.isPaused)
                {
                    yield return null;
                }
            }
            tween.runOnComplete();
        }

        //外部接口，生成并调用协程
        public static Transform DoMove(this Transform transform,Vector3 target,float duration)
        {
            Debug.Log("In function DoMove with target:"+target+" at time:"+Time.time);
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            MyTween tween = new MyTween("DoMove", target, duration, transform,null);
            Coroutine coroutine = mono.StartCoroutine(mono._DoMove(tween));
            tween.setCoroutine(coroutine);
            return tween.transform;
        }
    }

}
