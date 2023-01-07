using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private GameObject _muzleFlash;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _weaponPower;
    public void Shot()
    {
        _source.PlayOneShot(_shotSound);
        Instantiate(_muzleFlash,_source.gameObject.transform);
        //var hits = Physics.BoxCastAll(_source.gameObject.transform.position, _source.gameObject.transform.position, transform.forward,_source.gameObject.transform.rotation,9f,_layerMask);
        var hits = Physics.BoxCastAll(_source.gameObject.transform.position,new Vector3(.1f, .1f, 2f), _source.transform.forward, _source.gameObject.transform.rotation, 2f, _layerMask);
        foreach (var hit in hits)
        {
            Debug.Log(hit.transform.name);
            hit.transform.GetComponent<Zombie>().TakeDamage(_weaponPower);
        }
    }
}
