using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAttr{
    public float speed;
    public Vector3 originPosition;
    public UFOAttr(float _speed,Vector3 position)
    {
        speed = _speed;
        originPosition = position;
    }
}
