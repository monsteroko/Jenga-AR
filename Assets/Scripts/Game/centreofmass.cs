using UnityEngine;
using System.Collections;

public class centreofmass : MonoBehaviour
{
    public Vector3 com;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = com;
    }
}
