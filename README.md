
##目标
研究DOTWeen网站，实现Tween对象

## 参考资料
[学长博客1](https://www.jianshu.com/p/2a201125159a)
[学长博客2](http://marshallw.me/2017/05/14/unity%E5%AD%A6%E4%B9%A0%E7%AC%94%E8%AE%B0%EF%BC%88%E5%8D%81%E4%B8%89%EF%BC%89-dotween%E4%BB%BF%E5%86%99/)
[协程](https://blog.csdn.net/nizihabi/article/details/47606887)
[DOTween文档](http://dotween.demigiant.com/documentation.php#creatingTweener)

## 制作成品
[演示视频]()

测试代码
```C#
        transform.DoMove(new Vector3(5, 5, 5), 3.0f)
            .DoMove(new Vector3(0,0,0),3.0f)
            .DoScale(new Vector3(5,5,5),5.0f)
            .DoDelay(2)
            .OnComplete((tween) =>
            {
                tween.transform.DoMove(new Vector3(5, 0, 0), 3.0f);
            })
            .OnComplete((tween) =>
            {
                tween.transform.DoScale(new Vector3(0.5f, 0.5f, 0.5f), 3.0f);
            });
```

## 代码展示
结构：
MyTween.cs为实现的Tween类，MyExtension.cs为对Transform和Monobehaviour进行的扩展，TweenList.cs为针对transform外挂的列表，test.cs为测试脚本

1. 实现Tween的基本功能。
根据官方文档和师兄博客的说法，我觉得Tween的基本功能就是插值动画。所以我尝试实现了DoMove函数，效果还是很不错的。
```C#
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
```

2.实现回调函数
依照前面老师讲过的委托，据此实现了OnComplete方法，并将外部
```C#
        private Action<MyTween> _onComplete;

        public Transform OnComplete(Action<MyTween> callback)
        {
            _onComplete += callback;
            return transform;
        }
        public void runOnComplete()
        {
            if(this._onComplete != null)
            {
                _onComplete(this);
            }
            list.runOnComplete();
        }
```

3.实现链式法则
在阅读官方文档的时候，我看到这样的代码
```C#
myTween.SetLoops(4, LoopType.Yoyo).SetSpeedBased();
```
基于以前js上的开发经验，我觉得这样子的代码对于使用者而言会更方便一些，但是如果直接在原有代码基础上实现，因为返回的是Tween对象，势必会直接同时运行。
因为个人水平有限，我想到的方法是使用一个队列来维护每个对象的Tween，同时对队列中的Tween添加一个指向队列的引用。每当一个对象的Tween完成以后，在调用自身可能的OnComplete方法时也会去调用队列中的OnComplete方法。队列中的OnComplete方法在被调用的时候会尝试从自己的等待队列中取出排队的Tween对象，从而实现链式法则的顺序执行。
但是当我在实现这个队列的时候，我发现如果把它挂在Tween上是不可能的，只能够将它作为MonoBehaviour的继承类，作为组件挂载到GameObject上。在实现的时候如果返回值是Tween会有问题，所以我把返回值改成了Transform，同时将一些Tween上的方法扩展到Transform上。这样子表现上是差不多的。
问题：
1. 必须在脚本中先使用`transform.gameObject.AddComponent<TweenList>();`，不然会报错。
2. 因为使用`transform.gameObject.GetComponent<TweenList>().getTween()`的缘故，如果等待队列为空时很多方法会抛出异常。但是如果等待队列为空了，调用一些需要当前有正在运行Tween的方法是没有意义的。这样的行为无法进行避免。
总结：
1. 对设计模式和其他软件具体实现方法的了解还是不够，没什么好的想法。
2. 对C#的很多特性和面向对象的知识不是很了解，有种无从下手的感觉，还是要继续学习。