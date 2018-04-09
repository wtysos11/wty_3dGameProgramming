using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class UserClick : UnityEngine.EventSystems.EventTrigger {
    FirstSceneActionManager actionManager;
    ICharacterController characterController;
    public void setController(ICharacterController character)
    {
        characterController = character;
    }

    private void Start()
    {
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        actionManager = firstController.actionManager;
    }

    private void OnMouseDown()
    {


        //Debug.Log("onmousedown!");
        if (characterController==null)
        {
            actionManager.moveBoat();
        }
        else
        {
            FirstController firstController = Director.getInstance().currentSceneController as FirstController;
            firstController.clickCharacter(characterController);
        }
    }
}