using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Ÿ�Ͽ� Ÿ���� �Ǽ��Ǿ� �ִ��� �˻��ϴ� ������
    //�ڵ� ���� ������Ƽ�� ��������
    //�Ʒ��ڵ�ó�� �ۼ��ϸ� get,set�⺻���� �ڵ� �ۼ��� �� �� ��
    public bool IsBuildTower { set; get; }

    private void Awake()
    {
        IsBuildTower = false;
    }
}
