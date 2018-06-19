# 第十五周作业
使用状态图来实现牧师与魔鬼的智能帮助实现。
因为之前的Unity由于不明原因崩溃，所以下载了最新版的Unity，所以可能会带来一些问题，请谅解。
## 相关链接
[演示视频](https://www.bilibili.com/video/av25193698/)
## 具体实现
### 数据结构
 * 使用了邻接链表来实现图。点为状态点的模型，保存了当前状态点起始岸的信息（牧师数量、恶魔数量、船的数量），并且可以根据信息生成哈希码，也支持根据指定的哈希码解析对应信息。
 * 边为有向边，保存了起始点、终止点和实现的方式（有5种实现方式，派1个恶魔坐船、2个恶魔坐船、1个牧师坐船、2个牧师坐船、1个恶魔和1个牧师）
 * 同时点支持对当前状态合法与否的判断。
```C#
public class Node
{
    int state;
    int pr_num;//牧师数量
    int de_num;//恶魔数量
    int boat;
    public List<Edge> edge;
    public int parent;
    public Node(int pr_num, int de_num, int boat)
    {
        this.pr_num = pr_num;
        this.de_num = de_num;
        this.boat = boat;
        edge = new List<Edge>();
        state = getHash();
    }
    public Node(int hashCode)
    {
        state = hashCode;
        edge = new List<Edge>();
        decodeHash(hashCode);
    }

    public int getPr()
    {
        return pr_num;
    }

    public int getDe()
    {
        return de_num;
    }

    public int getBoat()
    {
        return boat;
    }

    public int getHash()
    {
        state = boat;
        state *= 4;
        state += pr_num;
        state *= 4;
        state += de_num;
        return state;
    }
    public static int getHash(int pr, int de, int b)
    {
        int ans;
        ans = b;
        ans *= 4;
        ans += pr;
        ans *= 4;
        ans += de;
        return ans;
    }

    public void decodeHash(int hash)
    {
        de_num = hash % 4;
        hash /= 4;
        pr_num = hash % 4;
        hash /= 4;
        boat = hash % 4;
    }

    //根据自身条件判断是否成立这样的状态点
    public bool judgeLegal()
    {
        //如果本岸或者对岸魔鬼的数量大于牧师，就会被吃掉
        if ((de_num > pr_num&&pr_num!=0) || ((3 - de_num) > (3-pr_num)&&pr_num!=3) || de_num < 0 || pr_num < 0 || de_num > 3 || pr_num > 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public class Edge
{
    public Node from;
    public Node to;
    public int state;
    public Edge(Node a,Node b,int state)
    {
        from = a;
        to = b;
        this.state = state;
    }
}
```

### 状态图的自动生成
使用BFS来生成状态图，使用哈希表`public Hashtable stateTable`来保存生成的新状态点并查重。键为使用Node内部函数生成的哈希编码。
```C#
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
            //Debug.Log(node.getPr()+" "+ node.getDe()+" "+ node.getBoat());
            int nowBoat;
            if(node.getBoat()==0)
            {
                nowBoat = 1;
            }
            else
            {
                nowBoat = 0;
            }
            Node[] arr = new Node[5];

            if(node.getBoat() == 1)//如果船在本岸，需要向对岸送人
            {
                arr[0] = new Node(node.getPr(), node.getDe() - 1, nowBoat);//d
                arr[1] = new Node(node.getPr(), node.getDe() - 2, nowBoat);//dd
                arr[2] = new Node(node.getPr() - 1, node.getDe() - 1, nowBoat);//dp
                arr[3] = new Node(node.getPr() - 1, node.getDe(), nowBoat);//p
                arr[4] = new Node(node.getPr() - 2, node.getDe(), nowBoat);//pp
            }
            else if(node.getBoat() == 0)//如果船在对岸，需要本岸接受
            {
                arr[0] = new Node(node.getPr(), node.getDe() + 1, nowBoat);//d
                arr[1] = new Node(node.getPr(), node.getDe() + 2, nowBoat);//dd
                arr[2] = new Node(node.getPr() + 1, node.getDe() + 1, nowBoat);//dp
                arr[3] = new Node(node.getPr() + 1, node.getDe(), nowBoat);//p
                arr[4] = new Node(node.getPr() + 2, node.getDe(), nowBoat);//pp
            }

        
            for(int i=0;i<5;i++)
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
```

### 使用算法实现下一步的计算
自动进行下一步，首先拿到当前状态的状态点，以此为初始点进行BFS来找到终点，然后根据结果回溯，找到前进的边，最后对前进边对应的操作予以执行，并检查游戏是否结束。
```C#
    public void next()
    {
        /** 目标：自动进行下一步
         *  首先找到当前状态
         *  以当前状态为初始点进行BFS，找到目标状态
         *  需要记录的信息：是否已经查找到，以及父亲节点
         *  找到目标以后进行反向，找到最开始的那条边，根据信息进行移动
         * 
         */
         //拿到目标状态点，初始化信息
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

//bfs查找路径
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
        switch(way)//根据状态进行操作
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
        }
        this.moveBoat();
        boat.allOnCoast();
        checkGameOver();
    }
```