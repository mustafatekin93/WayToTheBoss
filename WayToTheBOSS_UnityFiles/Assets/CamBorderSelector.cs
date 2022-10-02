using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBorderSelector : MonoBehaviour
{

    [SerializeField] private GameObject cinemachineCam;
    public Collider2D borderCollider;

    void Update()
    {
        var cam = cinemachineCam.GetComponent<Cinemachine.CinemachineConfiner>();
        cam.m_BoundingShape2D = borderCollider;
    }

    
}
