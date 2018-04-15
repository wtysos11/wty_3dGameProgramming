using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame; 
public class UserInterface : MonoBehaviour {
    public int status = 0;//0为正常模式
    /*
    正常模式下会在右上角维护一个分数label，（命中+1,未命中-1），一个重新开始的按钮
    */
    FirstController firstController;
    GUIStyle style;
    GUIStyle buttonStyle;
    private void Start()
    {
        this.firstController = Director.getInstance().currentSceneController as FirstController;

        style = new GUIStyle();
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
    }

    private void OnGUI()
    {
        //Debug.Log("function OnGUI with status:" + status);
        if(status == 0)
        {
            GUI.Label(new Rect(0,0,50,50), firstController.score.getScore().ToString(),style);
            /*
            if(GUI.Button(new Rect(0,60,140,70), "Restart", buttonStyle))
            {
                firstController.restart();
            }*/
        }
        /*
        else if(status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", style);
            if(GUI.Button(new Rect(Screen.width/2-70,Screen.height/2,140,70),"Restart",buttonStyle))
            {
                status = 0;
                firstController.restart();
            }
        }
        else if(status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                firstController.restart();
            }
        }*/
    }
}
