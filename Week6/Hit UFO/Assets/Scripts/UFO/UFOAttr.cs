using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAttr{
    public float scale;
    public float speed;
    public Vector3 originPosition;
    public UFOAttr(float _scale, float _speed,Vector3 position)
    {
        scale = _scale; speed = _speed;
        originPosition = position;
    }
}
