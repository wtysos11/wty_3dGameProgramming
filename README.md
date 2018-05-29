# 第十二周 粒子系统
## 制作内容
水枪灭火系统。有两个粒子系统，一个是水枪，一个是火苗。
[演示视频](https://www.bilibili.com/video/av24055418/)
## 实现内容
1.第一人称视角的水枪移动。
2.输入输出（鼠标滚轮控制水枪流量）
3.火苗的动态增减（根据流量计算）

## 具体实现
1.第一人称视角的水枪移动
这个其实前几次作业都有涉及到，具体的操作是把水枪和主摄像机绑在一起，然后通过鼠标输入动态改变物体的旋转角度。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCharacterController : MonoBehaviour
{
    private bool esc = false;
    //方向灵敏度  
    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    //上下最大视角(Y视角)  
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            esc = !esc;
        }
        //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //总体设置一下相机角度  
        if (!esc)
        {
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }

    void Start()
    {
        // Make the rigid body not change rotation  
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }
    }
}
```

2.输入输出
借助了Input.GetAxis("Mouse ScrollWheel")来获取鼠标滚动的值，前滚为正，后滚为负。经测试一般都是0.1到0.2左右，滚的比较快的时候会有0.3。
值得注意的是水枪流量是通过修改ParticalSystem.emission.rateOverTime来实现的，但是rateOverTime是一个比较奇怪的类型（ParticleSystem.MinMaxCurve），也就是说不能够直接转换它的值。官方文章中说是可以直接使用constantMax来访问值。具体要见[这篇文章](https://blogs.unity3d.com/cn/2016/04/20/particle-system-modules-faq/)，这是Unity官方写的Particle System Modules的FAQ，上面有很详细的说明。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalController : MonoBehaviour {

    public delegate void ChangeWithWater(int total);
    public static event ChangeWithWater OnChangeWithWater;

    ParticleSystem ps;
    float myMin = 1.0f;
    float myMax = 1000.0f;
    float myBase = 1000.0f;
    public int totalPartical = 0;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    public GameObject XFTarget;


    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        XFTarget = GameObject.FindGameObjectWithTag("flame");
        ps.trigger.SetCollider(0,XFTarget.GetComponent<BoxCollider>());
	}
	
	// Update is called once per frame
	void Update () {
        var em = ps.emission;
        float scale = em.rateOverTime.constantMax;
        if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            scale += (float)Input.GetAxis("Mouse ScrollWheel") * myBase;
            if (scale < myMin)
                scale = myMin;
            else if (scale > myMax)
                scale = myMax;
        }
        em.rateOverTime = new ParticleSystem.MinMaxCurve(scale);
	}

    void OnParticleTrigger()
    {
        int enterNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        totalPartical += enterNum;
        OnChangeWithWater(totalPartical);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
```

3.火苗的动态增减
火苗的高度其实是根据水枪流量来进行动态计算的，依据是现实生活中如果水不够的情况下灭火反而会使火燃烧地更旺，物理学的不好，可能是使上方空气降温下沉导致火场氧气浓度增加之类的。
实现的话是给火苗绑上了一个脚本，计算通过的粒子对于火苗高度的影响。火苗高度使用ParticalSystem.main.startLifetime来进行控制，实际效果还不错，就是无法减到0很麻烦。
因为涉及到两个粒子系统之间的信息传递，所以用了之前用过的消息传递机制，将水枪的OnChangeWithWater的接口暴露出来给MainController，让MainController找到FlameController把具体的实现函数绑上去。虽然没什么必要，但是挺有趣的。
通过的粒子数在ParticalController中实现（上方代码中的OnParticalTrigger）。通过设立trigger来判断进入trigger的粒子数目。值得一提的是多次试验后发现只有在脚本中动态设定才能够准确设定粒子系统的碰撞体，原因未知。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour {
    public ParticleSystem ps;
    // Use this for initialization
    void Start () {
        ps = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeWithWater(int total)
    {
        Debug.Log(total);
        var main = ps.main;
        if (total <= 1000)
        {
            main.startLifetime = (float)0.002 * total + 2;
        }
        else if (total > 1000 && total <= 3000 && main.startLifetime.constantMax-0.0f>0.001f)
        {
            main.startLifetime = (float)-0.002 * total + 6;
            if(main.startLifetime.constantMax - 0.0f < 0.03f)
            {
                main.startLifetime = 0;
            }
        }
    }
}
```
顺便吐个槽，这周作业太多了，3d游戏设计、现代操作系统的实验报告和期中设计，计算机组成原理的单周期CPU，操作系统的纸质作业和实验四，英语的两篇文章，数据库的第二次课程设计，老师们是不是商量好的，太狠了吧……