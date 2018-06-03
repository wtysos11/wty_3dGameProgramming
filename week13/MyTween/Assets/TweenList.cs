using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyTween
{
    /*
     与指定的transform绑定（绑定过程由用户负责，目前）
     维护一个等待队列queue
     维护一个正在运行的Tween
     目标：实现Link
         */
    public class TweenList:MonoBehaviour
    {
        Queue<MyTween> tweenQueue;//正在等待中的queue
        MyTween currentTween;//正在进行的Tween
        public TweenList()
        {
            tweenQueue = new Queue<MyTween>();
            currentTween = null;
        }

        public MyTween getTween()
        {
            return currentTween;
        }
        

        private void runTween(MyTween tween)
        {
            Coroutine coroutine = null;
            if(tween.name.Equals("DoMove"))
            {
                coroutine = tween.mono.StartCoroutine(tween.mono._DoMove(tween));
            }
            else if(tween.name.Equals("DoDelay"))
            {
                coroutine = tween.mono.StartCoroutine(tween.mono._DoDelay(tween));
            }
            else if(tween.name.Equals("DoScale"))
            {
                coroutine = tween.mono.StartCoroutine(tween.mono._DoScale(tween));
            }
            tween.setCoroutine(coroutine);
        }

        public void addTween(MyTween tween)
        {
            if(currentTween == null)
            {
                currentTween = tween;
                //运行currentTween
                runTween(tween);
            }
            else
            {
                tweenQueue.Enqueue(tween);
            }
        }

        public MyTween deleteTween()
        {
            if(tweenQueue.Count!=0)
            {
                return tweenQueue.Dequeue();
            }
            return null;
        }
        public void runOnComplete()
        {
            //对已完成的tween进行处理

            //装载新的Tween
            if(tweenQueue.Count!=0)
            {
                currentTween = tweenQueue.Dequeue();
                runTween(currentTween);
            }
            else
            {
                currentTween = null;
            }
        }
    }
}