using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace MyTween
{
    public static class MyExtension
    {
        public static IEnumerator _DoDelay(this MonoBehaviour mono,MyTween tween)
        {
            yield return new WaitForSeconds(tween.duration);
            tween.runOnComplete();
        }

        public static Transform DoDelay(this Transform transform,float sec)
        {
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            TweenList list = mono.transform.gameObject.GetComponent<TweenList>();
            MyTween tween = new MyTween("DoDelay", mono, new Vector3(0,0,0), sec, transform, null);
            list.addTween(tween);
            return tween.transform;

        }

        //具体的实现
        public static IEnumerator _DoMove(this MonoBehaviour mono,MyTween tween)
        {
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
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            TweenList list = mono.transform.gameObject.GetComponent<TweenList>();
            MyTween tween = new MyTween("DoMove",mono,target, duration, transform,null);
            list.addTween(tween);
            return tween.transform;
        }

        public static IEnumerator _DoScale(this MonoBehaviour mono,MyTween tween)
        {
            Vector3 dis = (tween.target - tween.transform.localScale) / tween.duration;
            for (float f = tween.duration; f >= 0.0f; f -= Time.deltaTime)
            {
                tween.transform.localScale += dis * Time.deltaTime;
                yield return null;

                while (tween.isPaused == true)
                {
                    yield return null;
                }
            }
            tween.runOnComplete();
        }

        public static Transform DoScale(this Transform transform,Vector3 targetScale,float duration)
        {
            MonoBehaviour mono = transform.GetComponents<MonoBehaviour>()[0];
            TweenList list = mono.transform.gameObject.GetComponent<TweenList>();
            MyTween tween = new MyTween("DoScale", mono, targetScale, duration, transform, null);
            list.addTween(tween);
            return tween.transform;
        }

        public static Transform OnComplete(this Transform transform, Action<MyTween> callback)
        {
            transform.gameObject.GetComponent<TweenList>().getTween().OnComplete(callback);
            return transform;
        }

        //问题：如果getTween()返回null会抛出异常
        public static Transform Pause(this Transform transform)
        {
            transform.gameObject.GetComponent<TweenList>().getTween().Pause();
            return transform;
        }

        public static Transform Play(this Transform transform)
        {
            transform.gameObject.GetComponent<TweenList>().getTween().Play();
            return transform;
        }
    }

}
