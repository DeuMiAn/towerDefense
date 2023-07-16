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

    //public float MoveSpeed => moveSpeed; //moveSpeed�� private �����ϱ� get�� �̷��� ������ �ä�
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);// Mathf.Max�� ����� �̵��ӵ��� ������ �ȵǵ��� ����
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
