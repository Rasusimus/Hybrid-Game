using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPossession : MonoBehaviour
{
    public bool keyTaken;
    private Vector3 originalPos;


    private void OnMouseDown()
    {
        keyTaken = true;
    }

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (keyTaken)
        {
            transform.position = originalPos;
        }
    }
}
