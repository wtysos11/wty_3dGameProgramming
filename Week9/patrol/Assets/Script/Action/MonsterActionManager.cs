using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class MonsterActionManager : BaseActionManager,ActionCallback
{
    private static int MAX_RANGE = 1;
    public void randomMove(MonsterController monster)
    {
        float moveSpeed = 1;
        Vector3 currentPos = monster.transform.position;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget3, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget4, moveSpeed);//前往位置4

        Vector3 randomTarget5 = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z);
        LineAction moveAction5 = LineAction.GetBaseAction(randomTarget5, moveSpeed);//前往位置5
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3, moveAction4,moveAction5 });//制作SequenceAction
        addAction(monster.gameObject, sequenceAction, this);
    }
    public void anotherRandomMove(MonsterController monster)
    {
        this.removeAction(monster.gameObject);
        float moveSpeed = 1;
        Vector3 currentPos = monster.transform.position;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - MAX_RANGE, 0),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(0, currentPos.z + MAX_RANGE)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget3, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(
            Random.Range(0, currentPos.x + MAX_RANGE),
            0,
            Random.Range(currentPos.z - MAX_RANGE, 0)
            );

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget4, moveSpeed);//前往位置4

        Vector3 randomTarget5 = new Vector3(monster.transform.position.x, monster.transform.position.y, monster.transform.position.z);
        LineAction moveAction5 = LineAction.GetBaseAction(randomTarget5, moveSpeed);//前往位置5
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3, moveAction4, moveAction5 });//制作SequenceAction
        addAction(monster.gameObject, sequenceAction, this);
    }

    public void actionDone(BaseAction source)
    {

    }


}
