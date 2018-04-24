using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class UserClick : UnityEngine.EventSystems.EventTrigger {
    ActionAdapter actionAdapter;

    private void Start()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        actionAdapter = firstController.actionAdapter;
    }

    private void OnMouseDown()
    {

    }
}