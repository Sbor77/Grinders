using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");        

        Vector3 offset = new Vector3(h, 0, v) * Time.deltaTime * 10f;

        transform.Translate(offset);
    }
}