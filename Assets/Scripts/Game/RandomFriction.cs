using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFriction : MonoBehaviour
{

    [Range(0f, 1f)]
    public float minFriction;

    [Range(0f, 1f)]
    public float maxFriction;

    void Start()
    {
        MeshCollider mainCollider = GetComponent<MeshCollider>();
        PhysicMaterial physicMaterial = mainCollider.material;
        physicMaterial.dynamicFriction = Random.Range(minFriction, maxFriction);
        physicMaterial.staticFriction = Random.Range(minFriction, maxFriction);
    }
}
