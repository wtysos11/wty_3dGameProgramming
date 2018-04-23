using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;
public class UFOActionManager : BaseActionManager,ActionCallback {

    public void ufoRandomMove(UFOObject ufo)
    {
        float moveSpeed = ufo.attr.speed;
        Vector3 currentPos = ufo.attr.originPosition;
        //注意修改
        Vector3 randomTarget1 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y+3),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );
        LineAction moveAction1 = LineAction.GetBaseAction(randomTarget1, moveSpeed);//前往位置1

        //目标位置2
        Vector3 randomTarget2 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y + 5),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );

        LineAction moveAction2 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置2

        //目标位置3
        Vector3 randomTarget3 = new Vector3(
            Random.Range(currentPos.x - 10, currentPos.x + 10),
            Random.Range(1, currentPos.y + 5),
            Random.Range(currentPos.z - 10, currentPos.z + 10)
            );

        LineAction moveAction3 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置3

        Vector3 randomTarget4 = new Vector3(ufo.ufo.transform.position.x, ufo.ufo.transform.position.y, ufo.ufo.transform.position.z);

        LineAction moveAction4 = LineAction.GetBaseAction(randomTarget2, moveSpeed);//前往位置4
        SequenceAction sequenceAction = SequenceAction.GetBaseAction(-1, 0, new List<BaseAction> { moveAction1, moveAction2, moveAction3,moveAction4 });//制作SequenceAction
        addAction(ufo.ufo, sequenceAction, this);
    }

    public void actionDone(BaseAction source)
    {

    }

}
