using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
        public Vector3 position;
    void Start()
    {
        //position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = this.transform.position;
    }
}
