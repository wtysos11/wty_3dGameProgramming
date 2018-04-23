using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class Score{
    private int record;
    private DifficultyManager difficulty;
    public Score()
    {
        record = 0;
        FirstController firstController = Director.getInstance().currentSceneController as FirstController;
        difficulty = firstController.difficultyManager;
    }
    public void clear()
    {
        record = 0;
    }
    public void update()
    {
        record = record + difficulty.getRank()+1;
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
