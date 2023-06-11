using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection=Vector3.zero;
  

    private bool isStop = false;

    public float MoveSpeed => moveSpeed; //moveSpeed는 private 변수니까 get을 이렇게 선언함 올ㅋ
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime* (isStop?0:1);
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection= direction;
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
