using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }

}
