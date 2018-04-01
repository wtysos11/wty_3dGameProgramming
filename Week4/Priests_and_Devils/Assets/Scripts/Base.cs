using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController currentSceneController { get; set; }//导演手中的场景控制器

        public static SSDirector getInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    //场记接口，实现由FirstController中进行
    public interface ISceneController
    {
        void LoadResources();
    }
    //用户界面与游戏模型的交互接口
    public interface IUserAction
    {
        void restart();
        void moveBoat();
        void clickCharacter(ICharacterController charctrl);
    }

    //移动控制器
    public class MoveController:MonoBehaviour
    {
        float speed = 20f;
        Vector3 destination;
        int status = 0;//0为结束，1为开始

        void Update()
        {
            if(status == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

                if(Vector3.Distance(transform.position, destination)<0.0001)
                {
                    status = 0;
                }
            }
        }

        public void Move(Vector3 dest)
        {
            destination = dest;
            status = 1;
        }
    }

    //用于河岸控制器的储存结构
    public class CoastStorage
    {
        private ICharacterController[] characterStorage = new ICharacterController[6];
        public bool isFull()
        {
            int counter = 0;
            for(int i=0;i<6;i++)
            {
                if (characterStorage[i] != null)
                    counter++;
            }
            if (counter == 6)
                return true;
            else
                return false;
        }
        //返回值是插入的位置
        public int insert(ICharacterController element)
        {
            if (this.isFull())
                return -1;

            for(int i=0;i<6;i++)
            {
                if(characterStorage[i]==null)
                {
                    characterStorage[i] = element;
                    return i;
                }
            }
            return -1;
        }

        public ICharacterController getCharacter(int pos)
        {
            if (pos < 0 || pos >= 6)
                return null;
            else
            {
                return characterStorage[pos];
            }
        }
        public bool remove(int pos)
        {
            if(characterStorage[pos]!=null)
            {
                characterStorage[pos] = null;
                return true;
            }
            else
                return false;
        }
        public void clear()
        {
            characterStorage = new ICharacterController[6];
        }
    }


    //两个岸的控制器，包括开始建立，确认是否结束。响应点击事件
    public class CoastController
    {
        readonly public GameObject coast;
        public CoastStorage storage;

        public CoastController(int status,Vector3 pos)
        {
            coast = Object.Instantiate(Resources.Load("Prefabs/stone", typeof(GameObject))) as GameObject;
            coast.name = "from_coast";
            if (status == 1)
            {
                coast.transform.position = pos;
                coast.name = "to_coast";
            }
        }

        public bool OnCoast(ICharacterController character)
        {
            if (storage.isFull())
                return false;
            else
            {
                storage.insert(character);
                return true;
            }
        }
        public bool OffCoast(int pos)
        {
            bool flag = storage.remove(pos);
            return flag;
        }
        public void reset()
        {
            storage.clear();
        }
    }

    //船的控制器，包括一个堆栈用于塞人
    public class BoatController
    {
        readonly public GameObject boat;
        readonly MoveController movescript;
        int boatStatus;//0为从fromCoast开到toCoast，1为开回来

        //两个ICharacter对象
        ICharacterController frontCharacter;
        ICharacterController backCharacter;

        //两个相对向量，表示相对于船的两个乘员的位置
        readonly Vector3 front = new Vector3(0.5f,0.5f,0);
        readonly Vector3 back = new Vector3(-0.5f, 0.5f, 0);
        public BoatController()
        {
            Debug.Log("boat controller init");
            boat = Object.Instantiate(Resources.Load("Prefabs/boat", typeof(GameObject))) as GameObject;
            movescript = boat.AddComponent(typeof(MoveController)) as MoveController;
            boat.AddComponent(typeof(UserClick));
            boatStatus = 0;
        }
        public void move()
        {
            Debug.Log("move");
            if (boatStatus == 0)
            {
                boatStatus = 1;
                movescript.Move(new Vector3(8, 0, 0));
            }
            else
            {
                boatStatus = 0;
                movescript.Move(new Vector3(4, 0, 0));
            }
        }
        
    }

    //人物控制器
    public class ICharacterController
    {
        readonly public GameObject character;
        readonly public string race;
        readonly UserClick userclick;
        bool _onBoat;
        public ICharacterController(int index,string racing,Vector3 pos)
        {
            string path = "Prefabs/" + racing;
            character = Object.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
            character.name = racing + index.ToString();
            character.transform.position = pos;
            race = racing;
            _onBoat = false;

            userclick = character.AddComponent(typeof(UserClick)) as UserClick;
            userclick.setController(this);
        }

    }
}