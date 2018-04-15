using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public enum BaseActionEventType : int { Started, Completed }
    public interface ActionCallback
    {
        void actionDone(BaseAction source);
    }
    //从ScriptableObject继承而来的基类
    public class BaseAction : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;
        
        public GameObject gameobject { get; set; }
        public Transform transform { get; set; }
        public ActionCallback callback { get; set; }

        protected BaseAction() { }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }

    //简单动作类
    public class LineAction : BaseAction
    {
        public Vector3 target;
        public float speed;

        //由Unity来创建动作类，保证内存正确回收
        public static LineAction GetBaseAction(Vector3 target,float speed)
        {
            LineAction action = ScriptableObject.CreateInstance<LineAction>();
            action.target = target;
            action.speed = speed;
            return action;
        }

        public override void Update()
        {
            //Debug.Log(this.transform == null);
            //Debug.Log("In Line Update");
            //Debug.Log("aim position:"+this.transform.position.ToString());
            //Debug.Log("target:"+target.ToString() + " speed:" + speed.ToString());
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if(this.transform.position == target)
            {
                this.destroy = true;
                this.callback.actionDone(this);
            }
        }

        public override void Start()
        {
            //暂时不实现
        }
    }

    //组合动作实现，例如折线动作
    public class SequenceAction : BaseAction, ActionCallback
    {
        public List<BaseAction> sequence;
        public int repeat = 1;//1 is act once, -1 is act forever
        public int current = 0;//pointer that points to the beginning

        //同简单动作
        public static SequenceAction GetBaseAction(int repeat,int point,List<BaseAction> sequence)
        {
            SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
            action.repeat = repeat;
            action.sequence = sequence;
            action.current = point;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if(current<sequence.Count)//执行当前动作
            {
                sequence[current].Update();
            }
        }
        public void actionDone(BaseAction source)
        {
            //Debug.Log("actionDone in SequenceAction");
            source.destroy = false;
            this.current++;
            if (this.current >= sequence.Count)
            {
                this.current = 0;
                if ( repeat > 0 ) repeat--;
                if ( repeat == 0 )
                {
                    this.destroy = true;
                    this.callback.actionDone(this);
                }
            }
        }

        //开始的时候注册所有动作
        public override void Start()
        {
            foreach(BaseAction action in sequence)
            {
                action.gameobject = this.gameobject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        void OnDestroy()
        {
            foreach(BaseAction action in sequence)
            {
                DestroyObject(action);
            }
        }
    }



    //动作管理基类
    public class BaseActionManager : MonoBehaviour
    {
        public Dictionary<int, BaseAction> actions = new Dictionary<int, BaseAction>();
        private List<BaseAction> addList = new List<BaseAction>();
        private List<int> deleteList = new List<int>();

        //动作做完之后自动回收动作
        protected void Update()
        {
            foreach(BaseAction action in addList)
            {
                actions[action.GetInstanceID()] = action;
            }
            addList.Clear();

            foreach(KeyValuePair<int,BaseAction> keyValue in actions)
            {
                BaseAction action = keyValue.Value;

                if (action.destroy)
                {
                    deleteList.Add(action.GetInstanceID());
                }
                else if(action.enable)
                {
                    action.Update();
                }
            }

            foreach(int key in deleteList)
            {
                BaseAction action = actions[key];
                actions.Remove(key);
                DestroyObject(action);
            }
            deleteList.Clear();
        }

        public void addAction(GameObject gameobject,BaseAction action,ActionCallback manager)
        {
            action.gameobject = gameobject;
            action.transform = gameobject.transform;
            action.callback = manager;
            action.Start();
            addList.Add(action);
        }

        public void removeAction(GameObject gameobject)
        {
            foreach(KeyValuePair<int, BaseAction> kv in actions)
            {
                if(kv.Value.gameobject == gameobject)
                {
                    kv.Value.destroy = true;
                    kv.Value.enable = false;
                }
            }
        }

        protected void Start()
        {
            //nothing to do in start
        }

    }

}
