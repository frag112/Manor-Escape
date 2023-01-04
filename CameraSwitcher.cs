using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _firstCamera, _secondCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_firstCamera.Priority< 10)
            {
                _firstCamera.Priority = 10;
                _secondCamera.Priority = 1;
            }
            else
            {
                _secondCamera.Priority = 10;
                _firstCamera.Priority = 1;
            }
        }
    }
}
