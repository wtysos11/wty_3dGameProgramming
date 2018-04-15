using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score{
    private int record;
    public Score()
    {
        record = 0;
    }
    public void clear()
    {
        record = 0;
    }
    public void update()
    {
        record++;
    }
    public int getScore()
    {
        return record;
    }
    public void fail()
    {
        record--;
    }
}
