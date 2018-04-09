using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController
{
    UserInterface userInterface;

    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    public ICharacterController[] characters;
    public FirstSceneActionManager actionManager;

    void Awake()
    {
        //导演单例模式加载
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userInterface = gameObject.AddComponent<UserInterface>() as UserInterface;
        actionManager = gameObject.AddComponent<FirstSceneActionManager>() as FirstSceneActionManager;
        Debug.Log(actionManager == null);
        this.LoadResources();
    }

    public void LoadResources()
    {
        GameObject water = Instantiate(Resources.Load("Prefabs/water",typeof(GameObject))) as GameObject;
        fromCoast = new CoastController(0,new Vector3(0,0,0));
        toCoast = new CoastController(1, new Vector3(12, 0, 0));
        boat = new BoatController();

        characters = new ICharacterController[6];
        for(int i=0;i<3;i++)
        {
            characters[i] = new ICharacterController(i,"priest",new Vector3((float)2.5-i,(float)1.25,0));
        }
        for(int i=3;i<6;i++)
        {
            characters[i] = new ICharacterController(i, "devil", new Vector3((float)2.5 - i, (float)1.25, 0));
        }
        fromCoast.initStorage(characters);
        /*
        Debug.Log("check whether:" + (fromCoast.storage.characterStorage[0] == fromCoast.storage.characterStorage[1]));
        for(int i=0;i<6;i++)
        {
            Debug.Log(fromCoast.storage.characterStorage[i].character.name);
        }*/
    }


    public void restart()
    {
        fromCoast.reset();
        toCoast.reset();
        boat.reset();
        for(int i=0;i<6;i++)
        {
            characters[i].reset();
            fromCoast.OnCoast(characters[i], 0);
        }
        
    }
    /*
    public void moveBoat()
    {
        if (userInterface.status != 0)
            return;

        //Debug.Log("boat");
        boat.move();
        if(boat.boatStatus == 0)//需要检查船移动是否造成游戏结束
        {
            if(fromCoast.check_over(boat) || toCoast.check_over())
            {
                userInterface.status = 1;
            }
        }
        else
        {
            if(fromCoast.check_over()|| toCoast.check_over(boat))
            {
                userInterface.status = 1;
            }

        }
    }*/
    public bool isBoatMove()
    {
        if (userInterface.status != 0)
            return false;
        else
            return true;
    }

    //根据当前状况判断是否存在游戏结束的可能，并修改userInterface中的参数以作用
    public void checkGameover()
    {
        if (boat.boatStatus == 0)//需要检查船移动是否造成游戏结束
        {
            if (fromCoast.check_over(boat) || toCoast.check_over())
            {
                userInterface.status = 1;
            }
        }
        else
        {
            if (fromCoast.check_over() || toCoast.check_over(boat))
            {
                userInterface.status = 1;
            }
        }
        //是否胜利
        if(toCoast.check_win())
        {
            userInterface.status = 2;
        }
    }

    public void clickCharacter(ICharacterController charctrl)
    {
        //legal check
        if((charctrl.place == "from" && boat.boatStatus == 1) || (charctrl.place == "to" && boat.boatStatus == 0))
        {
            return;
        }
        if (userInterface.status != 0)
            return;
        if (boat.boatFull()&&charctrl.onBoat == false)
            return;

        //Debug.Log(charctrl.character.name);
        CoastController whichCoast;
        if (boat.boatStatus == 0)
        {
            whichCoast = fromCoast;
        }
        else
        {
            whichCoast = toCoast;
        }
        //Debug.Log("check coastcontroller " + whichCoast.coast.name);
        //Debug.Log("check boatcontroller " + boat.boat.name);

        //检查是否合法

        //备注：上船与下船不对称，上船时船负责提供运动终点，下船时岸负责提供运动终点
        if (charctrl.onBoat == false)//上船的过程
        {
           // Debug.Log("function clickCharacter ready to go on boat with parameter:" + charctrl.character.name);
            whichCoast.OffCoast(charctrl);//离岸
            boat.OnBoat(charctrl);
        }
        else
        {
           // Debug.Log("function clickCharacter ready to go off boat with parameter:" + charctrl.character.name);
            whichCoast.OnCoast(charctrl,boat.boatStatus);//下船上岸
            boat.OffBoat(charctrl);
        }

        this.checkGameover();
    }

    //gameover时返回-1,胜利时返回1

}
