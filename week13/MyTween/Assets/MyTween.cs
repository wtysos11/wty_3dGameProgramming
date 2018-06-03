using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyTween
{
    /*
     MyTween的作用：
     1.保存运动信息
     2.保存当前运动的回调函数
         */
    public class MyTween
    {

        //运动信息
        internal string name;
        internal MonoBehaviour mono;
        internal Transform transform;
        internal Vector3 target;
        internal float duration;
        internal TweenList list;
        //协程
        internal Coroutine coroutine;
        //状态
        public bool isPaused;
        //回调函数
        private Action<MyTween> _onComplete;

        public MyTween OnComplete(Action<MyTween> callback)
        {
            this._onComplete += callback;
            return this;
        }

        //构造函数
        public MyTween(string name,MonoBehaviour mono,Vector3 target,float duration,Transform transform,Coroutine coroutine)
        {
            this.name = name;
            this.mono = mono;
            this.target = target;
            this.duration = duration;
            this.transform = transform;
            this.list = transform.gameObject.GetComponent<TweenList>();
            this.coroutine = coroutine;
            isPaused = false;
        }

        public void setCoroutine(Coroutine coroutine)
        {
            this.coroutine = coroutine;
        }

        public void runOnComplete()
        {
            if(this._onComplete != null)
            {
                _onComplete(this);
            }
            list.runOnComplete();
        }
    }
}