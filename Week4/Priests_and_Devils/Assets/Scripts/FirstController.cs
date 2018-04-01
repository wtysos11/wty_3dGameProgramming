using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    UserInterface userInterface;

    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    public ICharacterController[] characters;
    
    //坐标管理
    readonly Vector3 from_coast_origin = new Vector3((float)2.5, (float)1.25, 0);
    readonly Vector3 to_coast_origin = new Vector3((float)9.5, (float)1.25, 0);

    void Awake()
    {
        //导演单例模式加载
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;

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
    }


    public void restart()
    {

    }
    public void moveBoat()
    {
        //Debug.Log("boat");
    }
    public void clickCharacter(ICharacterController charctrl)
    {
        //Debug.Log(charctrl.character.name);
    }


}
