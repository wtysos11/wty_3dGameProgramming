using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UserInterface : MonoBehaviour
{
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

    }
}
