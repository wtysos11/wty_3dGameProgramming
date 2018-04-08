public class SSAction : ScriptableObject{
    public bool enable = true;
    public bool destory = false;
    
    public GameObject gameobject {get; set;}
    public Transform transform {get; set;}
    public ISSActionCallback callback {get; set;}

    protected SSAction(){} // 通过将构造函数设为protected来防止用户自己new对象

    public virtual void Start(){
        throw new System.NotImplementedException();
    } 

    public virtual void Update(){
        throw new System.NotImplementedException();
    }
}

//简单动作实现
public class CCMoveToAction : SSAction{
    public Vector3 target;
    public float speed;

    //让Unity来创建动作类，保证内存正确回收
    public static CCMoveToAction GetSSAction(Vector3 target,float speed){
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update(){
        this.transform.position = Vector3.MoveTowards(this.transform.position,...);
        if(this.transform.position == target){//已经到达
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start(){
        ...;
    }
}

//组合动作实现
public class CCSequenceAction : SSAction, ISSActionCallback{
    public List<SSAction> sequence;
    public int repeat = -1;//repeat forever
    public int start = 0;

    //创建一个动作顺序执行序列，-1表示无限循环，start开始动作
    public static CCSequenceAction GetSSAction(int repeat, int start ,List<SSAction> sequence){
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()//执行当前动作
    {
        if(sequence.Count == 0) return;
        if(start < sequence.Count){
            sequence[start].Update();
        }
    }
//收到当前动作执行完成，推动下一个动作，如果完成一次循环，减次数。如果完成，通知动作的管理者。
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,...){
        source.destory = false;
        this.start++;
        if(this.start>=sequence.Count){
            this.start = 0;
            if(repeat > 0) repeat--;
            if(repeat ==  0) {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

//执行动作前，为每个动作注入当前动作游戏对象，并将自己作为动作事件的接收者
    public override void Start(){
        foreach(SSAction action in sequence){
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }
    void OnDestory(){
        //如果自己被注销，应该释放自己管理的动作
    }
}

//动作事件接口定义
public enum SSActionEventType: int{Started,Completed}
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);
}

//动作管理基类
public class SSActionManager: MonoBehaviour{
    private Dictionary <int,SSaction> actions = new Dictionary <int,SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int> ();

    //update is called once per frame.
    //动作做完后会自动回收动作
    protected void Update(){
        foreach(SSAction ac in waitingAdd) actions [ac.GetInstanceID()] = ac;
        waitingAdd.Clear();
        //添加事件

        foreach(KeyValuePair <int,SSAction> kv in actions){
            SSAction ac = kv.Value;
//演示了复杂集合对象的使用
            if(ac.destory){
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if(ac.enable){
                ac.Update();
            }
        }

        foreach(int key in waitingDelete){
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

//提供了运行一个新动作的方法。该方法把游戏对象与动作进行绑定，并绑定该动作事件的消息接收者
    public void RunAction(GameObect gameobject,SSAction action,ISSActionCallback manager){
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    protected void Start()
    {
        //执行该动作的Start方法
    }
}

/*
实战动作管理：
职责：
    1.接收场景控制的命令
    2.管理动作的自动执行。
场景控制器和动作管理器的关系
    建议场景控制器在start中用GetComponent<T>()将它作为场景管理的一员
    后面的代码主要是演示动作管理，加入场景过程比较奇葩
    动作管理器不应该有模型的指示，游戏对象信息必须由场景管理器提供。

*/
public class CCActionManager : SSActionManager, ISSActionCallback{
    public FirstController sceneController;
    public CCMoveToAction moveToA,moveToB,moveToC,moveToD;

    protected new void Start(){
        sceneController = (FirstController)SSDirector.getInstance().currentSceneController;
        sceneController.actionManager = this;
        moveToA = CCMoveToAction.GetSSAction(new Vector3(5,0,0),1);
        this.RunAction(sceneController.move1,moveToA,this);
        moveToC=CCMoveToAction.GetSSAction(new Vector3(-2,-2,-2),1);
        moveToD=CCMoveToAction.GetSSAction(new Vector3(3,3,3),1);
        CCSequenceAction ccs = CCSequenceAction.GetSSAction(3,0,new List<SSAction> {moveToC,moveTOD});
        this.RunAction(sceneController.move2,ccs,this);
    }

    protected new void Update(){
        base.Update();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,...)
    {
        if(source == moveToA){
            moveToB = CCMoveToAction.GetSSAction(new Vector3(-5,0,0),1);
            this.RunAction(sceneController.move1,moveToB,this);
        }
        else if(source == moveToB){
            moveToA = CCMoveToAction.GetSSAction(new Vector3(5,0,0),1);
            this.RunAction(sceneController.move1,moveToA,this);
        }
    }
}