using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection=Vector3.zero;
    private float baseMoveSpeed;

  

    private bool isStop = false;

    //public float MoveSpeed => moveSpeed; //moveSpeed는 private 변수니까 get을 이렇게 선언함 올ㅋ
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);// Mathf.Max를 사용해 이동속도가 음수가 안되도록 설정
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime* (isStop?0:1);
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection= direction;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
    public void Stop()
    {
        isStop=true;
    }
    public void setPosition(Vector3 direction)
    {
        isStop = true;
        transform.position = direction;
    }
}
