using UnityEngine;
using System.Collections;

public class FirstView : MonoBehaviour
{
    private bool esc = false;
    //方向灵敏度  
    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    //上下最大视角(Y视角)  
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            esc = !esc;
        }
        //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //总体设置一下相机角度  
        if (!esc)
        {
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }

    void Start()
    {
        // Make the rigid body not change rotation  
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }
    }
}