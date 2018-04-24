using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class DifficultyManager{
    int rank = 0;
    UFOAttr[] attrs;
    public DifficultyManager()
    {
        attrs = new UFOAttr[5];
        attrs[0] = new UFOAttr(0.5f, new Vector3(50, 5, 50));
        attrs[1] = new UFOAttr(1f, new Vector3(50, 5, 50));
        attrs[2] = new UFOAttr(2f, new Vector3(50, 5, 50));
        attrs[3] = new UFOAttr(2.5f, new Vector3(50, 5, 50));
        attrs[4] = new UFOAttr(3f, new Vector3(50, 5, 50));
    }
    public void levelUp()
    {
        if(rank<4)
            rank++;
    }
    public void levelDown()
    {
        if (rank >= 1)
            rank--;
    }
    public UFOAttr getAttr()
    {
        return attrs[rank];
    }
    public void clear()
    {
        rank = 0;
    }
    public int getRank()
    {
        return rank;
    }
}
