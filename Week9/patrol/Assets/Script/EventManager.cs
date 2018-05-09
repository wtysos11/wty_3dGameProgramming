using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;

    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;

    public delegate void GameWin();
    public static event GameWin GamewinChange;

    public void PlayerEscape()
    {
        if (ScoreChange != null)
        {
            ScoreChange();
        }
    }

    public void PlayerGameover()
    {
        if(GameoverChange!=null)
        {
            GameoverChange();
        }
    }

    public void PlayerWin()
    {
        if (GamewinChange != null)
        {
            GamewinChange();
        }
    }
}
