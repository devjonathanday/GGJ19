using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    public bool grounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
            grounded = true;
        else grounded = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
            grounded = true;
        else grounded = false;
    }
    private void OnTriggerExit(Collider other)
    {
        grounded = false;
    }
}