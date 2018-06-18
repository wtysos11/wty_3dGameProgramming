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
    public Hashtable stateTable;
    
    void Awake()
    {
        //导演单例模式加载
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userInterface = gameObject.AddComponent<UserInterface>() as UserInterface;
        stateTable = new Hashtable();
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

        //生成状态图
        Debug.Log("Loading Resources");
        Queue<Node> q = new Queue<Node>();
        Node origin = new Node(3,3,1);//牧师3个，魔鬼3个，船在场，即开始状态
        q.Enqueue(origin);
        while(q.Count!=0)
        {
            Node node = q.Dequeue();

            if (stateTable.Contains(node.getHash()))//检查重复
            {
                continue;
            }

            stateTable.Add(node.getHash(), node);
            Debug.Log(node.getPr()+" "+ node.getDe()+" "+ node.getBoat());
            int nowBoat;
            if(node.getBoat()==0)
            {
                nowBoat = 1;
            }
            else
            {
                nowBoat = 0;
            }
            Node[] arr = new Node[6];

            if(node.getBoat() == 1)//如果船在本岸，需要向对岸送人
            {
                arr[0] = new Node(node.getPr(), node.getDe() - 1, nowBoat);//d
                arr[1] = new Node(node.getPr(), node.getDe() - 2, nowBoat);//dd
                arr[2] = new Node(node.getPr() - 1, node.getDe() - 1, nowBoat);//dp
                arr[3] = new Node(node.getPr() - 1, node.getDe(), nowBoat);//p
                arr[4] = new Node(node.getPr() - 2, node.getDe(), nowBoat);//pp
                arr[5] = new Node(node.getPr(), node.getDe(), nowBoat); // empty
            }
            else if(node.getBoat() == 0)//如果船在对岸，需要本岸接受
            {
                arr[0] = new Node(node.getPr(), node.getDe() + 1, nowBoat);//d
                arr[1] = new Node(node.getPr(), node.getDe() + 2, nowBoat);//dd
                arr[2] = new Node(node.getPr() + 1, node.getDe() + 1, nowBoat);//dp
                arr[3] = new Node(node.getPr() + 1, node.getDe(), nowBoat);//p
                arr[4] = new Node(node.getPr() + 2, node.getDe(), nowBoat);//pp
                arr[5] = new Node(node.getPr(), node.getDe(), nowBoat); // empty
            }

        
            for(int i=0;i<=5;i++)
            {
                var n = arr[i];
                if(n.judgeLegal())
                {
                    //判断是否为空船
                    if(n.getPr()==node.getPr()&&n.getDe()==node.getDe())
                    {
                        continue;
                    }

                    node.edge.Add(new Edge(node, n,i));
                    q.Enqueue(n);
                }
            }

        }
        
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

    //自动进行下一步
    public void next()
    {
        /** 目标：自动进行下一步
         *  首先找到当前状态
         *  以当前状态为初始点进行BFS，找到目标状态
         *  需要记录的信息：是否已经查找到，以及父亲节点
         *  找到目标以后进行反向，找到最开始的那条边，根据信息进行移动
         * 
         */
        int priest_num = fromCoast.getPriestNum();
        int devil_num = fromCoast.getDevilNum();
        int boatNum;
        if(boat.boatStatus==0)
        {
            boatNum = 1;
            priest_num += boat.getPriestNum();
            devil_num += boat.getDevilNum();
        }
        else
        {
            boatNum = 0;
        }
        int hashCode = Node.getHash(priest_num, devil_num, boatNum);
        Node current = stateTable[hashCode] as Node;
        Hashtable isVisit = new Hashtable();
        Queue<Node> bfs = new Queue<Node>();
        int endState = Node.getHash(0, 0, 0);


        bfs.Enqueue(current);
        while(bfs.Count!=0)
        {
            Node cur = bfs.Dequeue();
            if(cur.getHash()==endState)
            {
                break;
            }
            
            foreach(Edge e in cur.edge)
            {
                if(!isVisit.Contains(e.to.getHash()))
                {
                    Node aim = stateTable[e.to.getHash()] as Node;
                    aim.parent = cur.getHash();
                    isVisit.Add(aim.getHash(), aim);
                    bfs.Enqueue(aim);
                }
            }
        }

        //寻找正确的路径
        Node end = stateTable[endState] as Node;
        int pState = end.parent;
        while(pState!=hashCode)
        {
            end = stateTable[pState] as Node;
            pState = end.parent;
        }

        int way = 5;
        foreach(Edge e in current.edge)
        {
            if(e.to.getHash() == end.getHash())
            {
                way = e.state;
                break;
            }
        }
        //0 d 1 dd 2 dp 3 p 4 pp 5 empty
        boat.allOnCoast();
        CoastController whichCoast;
        if(boat.boatStatus==0)
        {
            whichCoast = fromCoast;
        }
        else
        {
            whichCoast = toCoast;
        }

        var storage = whichCoast.storage.characterStorage;
        switch(way)
        {
            case 0:
                ICharacterController one=null;
                foreach(var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("devil"))
                    {
                        one = characters;
                        break;
                    }
                }
                this.clickCharacter(one);
                break;
            case 1:
                ICharacterController one2 = null;
                foreach (var characters in storage)
                {
                    if(characters==null)
                    {
                        continue;
                    }

                    if (characters.race.Equals("devil"))
                    {
                        one2 = characters;
                        break;
                    }
                }
                this.clickCharacter(one2);
                ICharacterController two2 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("devil"))
                    {
                        two2 = characters;
                        break;
                    }
                }
                this.clickCharacter(two2);
                break;
            case 2:
                ICharacterController one3 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("devil"))
                    {
                        one3 = characters;
                        break;
                    }
                }
                this.clickCharacter(one3);
                ICharacterController two3 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("priest"))
                    {
                        two3 = characters;
                        break;
                    }
                }
                this.clickCharacter(two3);
                break;
            case 3:
                ICharacterController one4 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("priest"))
                    {
                        one4 = characters;
                        break;
                    }
                }
                this.clickCharacter(one4);
                break;
            case 4:
                ICharacterController one5 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("priest"))
                    {
                        one5 = characters;
                        break;
                    }
                }
                this.clickCharacter(one5);
                ICharacterController two5 = null;
                foreach (var characters in storage)
                {
                    if (characters == null)
                    {
                        continue;
                    }
                    if (characters.race.Equals("priest"))
                    {
                        two5 = characters;
                        break;
                    }
                }
                this.clickCharacter(two5);
                break;
            case 5:
                break;
        }
        this.moveBoat();
    }

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
        
        int flag = checkGameOver();
        Debug.Log("check game over:" + flag);
        if(flag == 1)
        {
            userInterface.status = 2;
        }
        else if(flag == -1)
        {
            userInterface.status = 1;
        }
    }

    //gameover时返回-1,胜利时返回1
    public int checkGameOver()
    {
        Debug.Log("check for game over");

        if(boat.boatStatus == 0)
        {
            if(fromCoast.check_over(boat)||toCoast.check_over())
            {
                return -1;
            }
        }
        else
        {
            Debug.Log("to coast check");
            if(fromCoast.check_over()||toCoast.check_over(boat))
            {
                return -1;
            }
        }

        if(toCoast.check_win())
        {
            return 1;
        }

        return 0;
    }


}
