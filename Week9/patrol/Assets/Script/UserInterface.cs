using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UserInterface : MonoBehaviour
{
    FirstSceneController firstController;
    GUIStyle style;
    GUIStyle buttonStyle;
    public int status = 0;
    private void Start()
    {
        this.firstController = Director.getInstance().currentSceneController as FirstSceneController;

        style = new GUIStyle();
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
    }

    private void OnGUI()
    {
        if(status == 1)
        {
            string ans = "game is over, your score is " + firstController.score;
            GUI.Label(new Rect(Screen.width / 2 - 50, 20, 70, 70), ans, style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50 , Screen.height / 2 - 180, 140, 70), "Restart", buttonStyle))
            {
                firstController.restart();
                status = 0;
            }
        }
    }
}
