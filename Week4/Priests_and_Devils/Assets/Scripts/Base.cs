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

    //两个岸的控制器，包括开始建立，确认是否结束。响应点击事件
    public class CoastController
    {

    }

    //船的控制器，包括一个堆栈用于塞人
    public class BoatController
    {

    }

    //人物控制器
    public class ICharacterController
    {

    }
}