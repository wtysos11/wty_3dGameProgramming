using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class UserClick : UnityEngine.EventSystems.EventTrigger {
    FirstSceneActionManager actionManager;

    private void Start()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        actionManager = firstController.actionManager;
    }

    private void OnMouseDown()
    {
        if (actionManager.canClick == false)
            return;

        //Debug.Log("onmousedown!");
        if (characterController==null)
        {
            actionManager.moveBoat();
        }
        else
        {
            actionManager.clickCharacter(characterController);
        }
    }
}