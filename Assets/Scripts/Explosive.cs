using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D explosiveSensor;

    public void onSensorOn()
    {
        explosiveSensor.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        print("Æø¹ßÀÌ´Ù!");
    }


}
