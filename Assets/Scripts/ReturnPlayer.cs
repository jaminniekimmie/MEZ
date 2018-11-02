using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPlayer : MonoBehaviour {

    public Transform ReturnPoint;

    public void OnTriggerEnter(Collider other)
    {
        other.transform.position = ReturnPoint.position;
    }
}
