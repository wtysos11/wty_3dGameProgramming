using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFind : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

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