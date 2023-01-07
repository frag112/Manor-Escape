using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _stopHere = .1f;
    [SerializeField] private bool _attack = false;
    [SerializeField] private float _health;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _aHit, _aStep, _aDead, _aPlayerSpotted, _aIdle;
    private void Start()
    {
        if (_attack)
        {
            MoveToTarget();
        }

    }
    private void StopWalking()
    {
        _animator.SetBool("Walking", false);
        StopCoroutine(Walk());
        _agent.isStopped= true;
    }
    protected void Bite()
    {
        StopWalking();
        if (_target.GetComponent<PlayerAnimationStateController>().Death(transform.position))
        {
            _animator.SetTrigger("Bite");
        }
        else
        {
            _animator.SetTrigger("Eat");
        }
    }
    public void MoveToTarget()
    {
        if (_health <= 0) return; 

        _agent.isStopped = false;
        StartCoroutine(Walk());
        _animator.SetBool("Walking", true);
    }
    protected IEnumerator Walk()
    {
        while ((_target != null) && ((int)Vector3.Distance(transform.position, _target.transform.position) > _stopHere))
        {
            _agent.destination = _target.transform.position;
            yield return new WaitForSeconds(1);
        }
        Bite();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _source.PlayOneShot(_aPlayerSpotted);
            MoveToTarget();
        }
    }
    public void TakeDamage(float damage)
    {
        _source.PlayOneShot(_aHit);
        _health -= damage;
        _animator.SetTrigger("Hit");
        StopWalking();
        if (_health > 0) return;
        Die();
    }
    private void Die()
    {
        _source.PlayOneShot(_aDead);
        gameObject.GetComponent<Collider>().enabled = false;
        _animator.SetBool("Dead", true);
        StopWalking();
        Destroy(this);
    }
}
