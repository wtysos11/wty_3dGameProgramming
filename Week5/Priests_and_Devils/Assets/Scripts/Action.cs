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

        protected void Start()
        {
            //nothing to do in start
        }

    }

    //本地管理特化类
    public class FirstSceneActionManager : BaseActionManager, ActionCallback
    {
        private float boatSpeed = 20f;
        private float personSpeed = 30f;
        private float resetSpeed = 100f;
        private FirstController firstController;
        private ICharacterController characterCache;
        private CoastController coastCache;
        int status = 0;//0为初始化状态，1为船的运动状态，2为人物运动状态
        readonly Vector3 frontmiddle1 = new Vector3(3.5f, 1.25f, 0);
        readonly Vector3 frontmiddle2 = new Vector3(3.5f, 0.5f, 0);
        readonly Vector3 backmiddle1 = new Vector3(8.5f, 1.25f, 0);
        readonly Vector3 backmiddle2 = new Vector3(8.5f, 0.5f, 0);

        //两个相对向量，表示相对于船的两个成员的位置（以from岸为基础）
        readonly Vector3 front = new Vector3(0.5f, 0.5f, 0);
        readonly Vector3 back = new Vector3(-0.5f, 0.5f, 0);

        public bool canClick;
        protected new void Start()
        {
            firstController = Director.getInstance().currentSceneController as FirstController;
            canClick = true;
        }

        public void moveBoat()
        {
            BoatController boat = firstController.boat;
            if (firstController.isBoatMove() == false||boat.boatEmpty()==true)
                return;

            status = 1;
            this.canClick = false;
            LineAction action = LineAction.GetBaseAction(boat.getDestination(),boatSpeed);
            this.addAction(boat.boat, action, this);
        }

        public void clickCharacter(ICharacterController character)
        {
            bool ok=firstController.isCharacterMove(character);
            if (ok == false)
                return;

            this.canClick = false;
            status = 2;
            characterCache = character;
            coastCache = firstController.getCharacterCoast();

            if (character.onBoat == false)//上船的过程
            {
                // Debug.Log("function clickCharacter ready to go on boat with parameter:" + charctrl.character.name);
                
                this.characterAction(coastCache,character,false);
            }
            else
            {
                // Debug.Log("function clickCharacter ready to go off boat with parameter:" + charctrl.character.name);
                
                this.characterAction(coastCache, character, true);
            }
            firstController.checkGameover();
        }

        //status为false时表示上船，status为true时表示下船
        public void characterAction(CoastController whichCoast,ICharacterController character,bool status)
        {
            List<BaseAction> actionList = new List<BaseAction>();
            BoatController boat = firstController.boat;
            LineAction action1=null, action2=null, targetAction=null;
            //制作序列表格actionList
            if (status == false)//上船
            {
                Vector3 relativeVec = this.getBoatEmpty(boat);
                //Debug.Log(relativeVec.ToString());
                if(boat.boatStatus == 0)
                {
                    action1 = LineAction.GetBaseAction(frontmiddle1, personSpeed);
                    action2 = LineAction.GetBaseAction(frontmiddle2, personSpeed);
                    
                }
                else
                {
                    action1 = LineAction.GetBaseAction(backmiddle1, personSpeed);
                    action2 = LineAction.GetBaseAction(backmiddle2, personSpeed);
                    
                }
                targetAction = LineAction.GetBaseAction(boat.boat.transform.position+relativeVec,personSpeed);
            }
            else if(status == true)
            {
                Vector3 relativeVec;
                int pos = whichCoast.getEmpty();
                if (whichCoast.coast.name == "from_coast")
                {
                    relativeVec = new Vector3(2.5f - pos, 1.25f, 0);
                }
                else
                {
                    relativeVec = new Vector3(-2.5f + pos, 1.25f, 0);
                }

                if (boat.boatStatus == 0)
                {
                    action1 = LineAction.GetBaseAction(frontmiddle2, personSpeed);
                    action2 = LineAction.GetBaseAction(frontmiddle1, personSpeed);
                    
                }
                else
                {
                    action1 = LineAction.GetBaseAction(backmiddle2, personSpeed);
                    action2 = LineAction.GetBaseAction(backmiddle1, personSpeed);
                    
                }
                targetAction = LineAction.GetBaseAction(whichCoast.coast.transform.position + relativeVec, personSpeed);
            }
            //注册复杂事件
            /*
            Debug.Log("register information:");
            Debug.Log("action1:" + action1.target.ToString());
            Debug.Log("action2:" + action2.target.ToString());*/
            //Debug.Log("targetAction:" + targetAction.target.ToString());
            actionList.Add(action1);
            actionList.Add(action2);
            actionList.Add(targetAction);
            SequenceAction action = SequenceAction.GetBaseAction(1, 0, actionList);
            /*
            this.addAction(character.character, action1, this);
            this.addAction(character.character, action2, this);
            this.addAction(character.character, targetAction, this);*/
            this.addAction(character.character, action, this);
            
        }

        public Vector3 getBoatEmpty(BoatController boat)
        {
            if(boat.boatStatus == 0)
            {
                if (boat.frontCharacter == null)
                {
                    return front;
                }

                else
                {
                    return back;
                }

            }
            else
            {
                if (boat.backCharacter == null)
                    return back;
                else
                    return front;
            }
        }

        public void reset()
        {
            BoatController boat = firstController.boat;
            LineAction action = LineAction.GetBaseAction(new Vector3(4, 0, 0), boatSpeed);
            this.addAction(boat.boat, action, this);

            ICharacterController[] characters = firstController.characters;
            for(int i=0;i<characters.Length;i++)
            {
                Vector3 relativeVec = new Vector3(2.5f - i, 1.25f, 0);
                LineAction chaAction = LineAction.GetBaseAction(firstController.fromCoast.coast.transform.position + relativeVec, resetSpeed);
                this.addAction(characters[i].character, chaAction, this);
            }
            this.canClick = true;
            this.status = 0;
        }

        public void actionDone(BaseAction source)
        {
            //Debug.Log("actionDone in FirstSceneActionManager");
            if(status == 1)
            {
                BoatController boat = firstController.boat;
                boat.switchBoatStatus();
            }
            else if(status == 2)
            {
                BoatController boat = firstController.boat;
                if (characterCache.onBoat == false)//上船
                {
                    boat.OnBoat(characterCache);
                    coastCache.OffCoast(characterCache);//离岸
                    characterCache.character.transform.parent = boat.boat.transform;
                }
                else
                {
                    boat.OffBoat(characterCache);
                    coastCache.OnCoast(characterCache, boat.boatStatus);//下船上岸
                    characterCache.character.transform.parent = null;
                }
            }
            canClick = true;
            firstController.checkGameover();
            status = 0;
        }
    }
}
