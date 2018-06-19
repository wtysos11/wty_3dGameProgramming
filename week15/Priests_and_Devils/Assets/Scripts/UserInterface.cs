using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame; 
public class UserInterface : MonoBehaviour {
    public int status = 0;//0为没有完成，1为失败，2为胜利
    private IUserAction action;
    GUIStyle style;
    GUIStyle buttonStyle;
    private void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }

    private void OnGUI()
    {
        //Debug.Log("function OnGUI with status:" + status);
        if(status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "Gameover!", style);
            if(GUI.Button(new Rect(Screen.width/2-70,Screen.height/2,140,70),"Restart",buttonStyle))
            {
                status = 0;
                action.restart();
            }
        }
        else if(status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                action.restart();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height -300, 140, 70), "Next", buttonStyle))
            {
                action.next();
            }
        }
    }
}
