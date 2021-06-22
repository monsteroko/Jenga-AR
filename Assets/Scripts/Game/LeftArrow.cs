using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour
{
    public MeshRenderer _renderer;
    public GameObject table;

    private void OnMouseDrag()
    {
        _renderer.material.color = Color.yellow;
        var objs = GameObject.FindGameObjectsWithTag("JengaPiece");
        for (int i = 0; i < objs.Length; i++)
            objs[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        table.transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime);
    }
    private void OnMouseUp()
    {
        var objs = GameObject.FindGameObjectsWithTag("JengaPiece");
        for (int i = 0; i < objs.Length; i++)
            objs[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _renderer.material.color = Color.green;
    }
}
