using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class Director : System.Object
    {
        private static Director _instance;
        public ISceneController currentSceneController { get; set; }//导演手中的场景控制器
                                                                    //坐标管理

        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }
    }
    
    //场记接口，实现由FirstController中进行
    public interface ISceneController
    {
        void LoadResources();
    }


 
}