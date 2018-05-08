using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UserInterface : MonoBehaviour
{
    FirstSceneController firstController;
    GUIStyle style;
    GUIStyle buttonStyle;
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

    }
}
