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
        public BoatController()
        {
            Debug.Log("boat controller init");
            boat = Object.Instantiate(Resources.Load("Prefabs/boat", typeof(GameObject))) as GameObject;
            boat.AddComponent(typeof(UserClick));
        }
    }

    //人物控制器
    public class ICharacterController
    {
        readonly public GameObject character;
        readonly public string race;
        readonly UserClick userclick;
        public ICharacterController(int index,string racing,Vector3 pos)
        {
            string path = "Prefabs/" + racing;
            character = Object.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
            character.name = racing + index.ToString();
            character.transform.position = pos;
            race = racing;

            userclick = character.AddComponent(typeof(UserClick)) as UserClick;
            userclick.setController(this);
        }

    }
}