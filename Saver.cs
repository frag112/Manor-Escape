using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public bool load = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (load == false)
            {
                var player = other.GetComponent<Player>();
                SaveSystem.SavePlayer(player);
                Debug.Log("you win");
            }
            else
            {
                PlayerData _data = SaveSystem.LoadPlayer();
                other.GetComponent<Player>().health = _data.health;
                Vector3 position;
                position.x = _data.position[0];
                position.y = _data.position[1];
                position.z = _data.position[2];
                other.transform.position = position;
                Debug.Log("you loaded");
            }

        }
    }
}
