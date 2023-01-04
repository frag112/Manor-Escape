using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _stopHere = .1f;
    [SerializeField] private bool _attack = false;
    [SerializeField] private float _health;
    private void Start()
    {
        if (_attack)
        {
            MoveToTarget();
        }

    }
    private void Update()
    {
        
    }
    protected void Bite()
    {
        _animator.SetBool("Walking", false);    
        if (_target.GetComponent<PlayerAnimationStateController>().Death(transform.position))
        {
            _animator.SetTrigger("Bite");
        }
        else
        {
            _animator.SetTrigger("Eat");
        }
    }
    protected void MoveToTarget()
    {
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
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health > 0) return;
        Die();
    }
    private void Die()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        _agent.isStopped = true;
        _animator.SetBool("Dead", true);
    }
}
