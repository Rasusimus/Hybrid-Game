using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private bool interacted;

    private Vector3 originalPos;
    private Vector3 newPos;

    [SerializeField] private float newPosZ = -1.4f;

    [SerializeField] private float speed = 2f; // Adjustable speed in the editor
    private float t = 0f; // Time tracker

    private bool rotated;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float targetRotation;
    private float originalRotationX;

    [SerializeField] private Interaction otherObject;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRotationX = transform.parent.localEulerAngles.x;
    }

    private void OnMouseDown()
    {
        t = 0f;

        if (!interacted)
        {
            interacted = true;
            newPos = new Vector3(originalPos.x, originalPos.y, newPosZ);
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

        float currentRotationX = transform.parent.localEulerAngles.x;

        if (currentRotationX > 180f) currentRotationX -= 360f;

        if (interacted && !rotated && currentRotationX > targetRotation)
        {
            float step = rotationSpeed * Time.deltaTime;
            float newRotationX = Mathf.MoveTowardsAngle(currentRotationX, targetRotation, step);
            transform.parent.localEulerAngles = new Vector3(newRotationX, transform.parent.localEulerAngles.y, transform.parent.localEulerAngles.z);

            if (Mathf.Approximately(newRotationX, targetRotation)) rotated = true;
        }
        else if (!interacted && otherObject != null && !otherObject.IsInteracted() && rotated)
        {
            float step = rotationSpeed * Time.deltaTime;
            float newRotationX = Mathf.MoveTowardsAngle(currentRotationX, originalRotationX, step);
            transform.parent.localEulerAngles = new Vector3(newRotationX, transform.parent.localEulerAngles.y, transform.parent.localEulerAngles.z);

            if (Mathf.Approximately(newRotationX, originalRotationX)) rotated = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            interacted = false;
        }
    }
    public bool IsInteracted() { return interacted; }
}
