using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCharacterController : MonoBehaviour {
    public float rotateSpeed = 16.0F;
    public float runningSpeed = 10.0F;//running speed
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(this.transform.position.y<-10)
        {
            Singleton<EventManager>.Instance.PlayerWin();
        }

        float running = Input.GetAxis("Vertical") * runningSpeed;
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed;
        if (running != 0 || rotate != 0)
        {
            animator.ResetTrigger("Rest");
        }
        else
        {
            animator.SetTrigger("Rest");
            return;
        }
        running *= Time.deltaTime;
        rotate *= Time.deltaTime;
        rotate *= 20;
        //Debug.Log("rotate:"+rotate);
        transform.Translate(0, 0, running);
        transform.Rotate(0, rotate, 0);
        animator.SetFloat("Speed", running);
        
    }



}
