using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class UserClick : UnityEngine.EventSystems.EventTrigger {
    IUserAction action;
    ICharacterController characterController;
    public void setController(ICharacterController character)
    {
        characterController = character;
    }

    private void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    private void OnMouseDown()
    {


        //Debug.Log("onmousedown!");
        if (characterController==null)
        {
            action.moveBoat();
        }
        else
        {
            action.clickCharacter(characterController);
        }
    }
}