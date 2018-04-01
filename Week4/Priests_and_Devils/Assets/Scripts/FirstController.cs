using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        this.LoadResources();
    }

    public void LoadResources()
    {
        
    }


    public void restart()
    {

    }
    public void moveBoat()
    {

    }
    public void clickCharacter(ICharacterController charctrl)
    {

    }


}
