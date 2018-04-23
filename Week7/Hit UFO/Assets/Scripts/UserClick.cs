using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class UserClick : UnityEngine.EventSystems.EventTrigger {
    UFOActionManager actionManager;

    private void Start()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        actionManager = firstController.actionManager;
    }

    private void OnMouseDown()
    {

    }
}