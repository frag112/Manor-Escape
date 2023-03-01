using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour
{
    [SerializeField] bool _doorIsOpen = false;
    [SerializeField] Rigidbody _door;
    [SerializeField] string _key;
    [SerializeField] GameObject _lock;
    [SerializeField] AudioSource _soundPlayer;
    [SerializeField] AudioClip _unlockSound;

    private void Awake()
    {
 
    }
    private void OnTriggerEnter(Collider other)
    {

            if ( !_doorIsOpen && other.CompareTag("Player"))
            {

                if (other.GetComponent<PlayerAnimationStateController>().UseItem(_key))
                {
                if(_door != null)
                {
                    _door.GetComponent<Rigidbody>().isKinematic = false;
                    _soundPlayer.PlayOneShot(_unlockSound);
                    _doorIsOpen = true;
                }
                    

                }
            }
    }
}
