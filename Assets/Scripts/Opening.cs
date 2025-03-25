using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour // WORK IN PROGRESS, ROTATION ON THE LEFT CABINET IS NOT WORKING PROPERLY
{
    private bool interacted;

    public KeyPossession keyPossession;

    private Vector3 originalPos;
    [Header("Positioning")]
    [SerializeField] private Vector3 newPos;
    [SerializeField] private float speed = 2f; // Adjustable speed in the editor

    private Vector3 originalRot;
    [Header("Rotation")]
    [SerializeField] private float targetY;
    [SerializeField] private float rotationSpeed;
    private bool rotated = false;

    private float t = 0f; // Time tracker

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localEulerAngles;
    }

    private void OnMouseDown()
    {
        t = 0f;

        bool keyTaken = keyPossession.keyTaken;

        if (!interacted && keyTaken)
        {
            interacted = true;
            // newPos = new Vector3(originalPos.x, originalPos.y, 1f);
        }
        else
        {
            interacted = false;
        }
    }

    void Update()
    {
        if (interacted && transform.localPosition != newPos)
        {
            t += Time.deltaTime * speed; // Increment time factor

            transform.localPosition = Vector3.Lerp(originalPos, newPos, t);
        }
        else if (!interacted)
        {
            t += Time.deltaTime * speed;

            transform.localPosition = Vector3.Lerp(newPos, originalPos, t);
        }
        // FIX THE ROTATION

        if (interacted && !rotated)
        {
            float step = rotationSpeed * Time.deltaTime;
            float targetYNormalized = NormalizeAngles(targetY);
            float targetRotationY = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, targetYNormalized, step);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetRotationY, transform.localEulerAngles.z);

            if (Mathf.Approximately(targetRotationY, (targetY))) rotated = true;
            Debug.Log(targetRotationY + " " + targetY);
            Debug.Log(rotated);
        }
        else if (!interacted && rotated)
        {
            float step = rotationSpeed * Time.deltaTime;
            float originalNormalized = NormalizeAngles(originalRot.y);
            float targetRotationY = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, originalNormalized, step);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetRotationY, transform.localEulerAngles.z);

            if (Mathf.Approximately(targetRotationY, originalRot.y)) rotated = false;
        }
    }
    float NormalizeAngles(float angle)
    {
        return (angle > 180f) ? angle - 360f : angle;
    }
}
