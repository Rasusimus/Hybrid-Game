using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    [SerializeField] private Transform model;

    [SerializeField] private Transform cam;

    [SerializeField] private Transform bground;

    [SerializeField] private Transform player;
    private Rigidbody2D rb;

    private SpriteRenderer object_sprite;
    private int order;

    [Header("Size")]
    [SerializeField] float maxSize;
    [SerializeField] float initialSize;

    private Vector3 maxScale;
    private Vector3 initialScale;
    private float growthDuration = 1f;

    private float elapsedTime = 0f;

    private Vector3 offset;
    private Vector3 originalPos;

    private bool isInFront = false;
    private bool escapeKeyPressed = false;
    private bool clicked = false;

    [Header("Rotation")]
    [SerializeField] private float maxRotation;
    [SerializeField] private bool vertical;

    private bool[] keyStates = new bool[4]; // 0 = A, 1 = D, 2 = W, 3 = S

    private float leftOrRight = 1f;
    private float upOrDown = 1f;
    private float rotationSpeed = 100f;

    private bool proximity = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") { proximity = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") { proximity = false; }
    }

    private void OnMouseDown()
    {
        if (!escapeKeyPressed && !isInFront && proximity && rb.bodyType != RigidbodyType2D.Static)
        {
            clicked = true;
        }
    }

    private void Start()
    {
        originalPos = model.position;

        offset = new Vector3(transform.position.x, transform.position.y, -1000f);
        maxScale = new Vector3(maxSize, maxSize, maxSize);
        initialScale = new Vector3(initialSize, initialSize, initialSize);

        model.localScale = initialScale;
        rb = player.GetComponent<Rigidbody2D>();

        object_sprite = GetComponent<SpriteRenderer>();
        order = object_sprite.sortingOrder;
    }

    void Update()
    {
        if (clicked && !isInFront)
        {
            rb.bodyType = RigidbodyType2D.Static;

            object_sprite.sortingOrder = 0;
            model.localRotation = Quaternion.Euler(0, 0, 0);

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / growthDuration;

            t = Mathf.Clamp01(t);

            model.position = offset;
            model.localScale = Vector3.Lerp(initialScale, maxScale, t);

            if (t >= 1)
            {
                isInFront = true;
                elapsedTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInFront)
        {
            escapeKeyPressed = true;
        }

        if (escapeKeyPressed) 
        { 
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / growthDuration;

            t = Mathf.Clamp01(t);

            model.localScale = Vector3.Lerp(maxScale, initialScale, t);

            if (t >= 1)
            {
                isInFront = false;
                clicked = false;
                escapeKeyPressed = false;

                rb.bodyType = RigidbodyType2D.Dynamic;

                model.position = originalPos;
                elapsedTime = 0;

                object_sprite.sortingOrder = order;
            }
        }

        if (isInFront)
        {
            float currentYRotation = model.localEulerAngles.y;
            if (currentYRotation > 180f) { currentYRotation -= 360f; }
            float currentXRotation = model.localEulerAngles.x;
            if (currentXRotation > 180f) { currentXRotation -= 360f; }

            keyStates[0] = Input.GetKey(KeyCode.A);
            keyStates[1] = Input.GetKey(KeyCode.D);
            keyStates[2] = Input.GetKey(KeyCode.W);
            keyStates[3] = Input.GetKey(KeyCode.S);

            if (keyStates[0])
            {
                keyStates[1] = false;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                keyStates[0] = false;
            }

            if (keyStates[1])
            {
                keyStates[0] = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                keyStates[1] = false;
            }

            if (keyStates[2])
            {
                keyStates[3] = false;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                keyStates[2] = false;
            }

            if (keyStates[3])
            {
                keyStates[2] = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                keyStates[3] = false;
            }

            float rotationY = leftOrRight * rotationSpeed * Time.deltaTime;
            float rotationX = upOrDown * rotationSpeed * Time.deltaTime;

            if (keyStates[0])
            {
                leftOrRight = 1f;
                if (currentYRotation < maxRotation)
                {
                    model.Rotate(Vector3.up, rotationY, Space.World);
                }
            }
            if (keyStates[1])
            {
                leftOrRight = -1f;
                if (currentYRotation > -maxRotation)
                {
                    model.Rotate(Vector3.up, rotationY, Space.World);
                }
            }

            if (vertical)
            {
                if (keyStates[2])
                {
                    upOrDown = 1f;
                    if (currentXRotation < maxRotation)
                    {
                        model.Rotate(Vector3.right, rotationX, Space.World);
                    }
                }
                if (keyStates[3])
                {
                    upOrDown = -1f;
                    if (currentXRotation > -maxRotation)
                    {
                        model.Rotate(Vector3.right, rotationX, Space.World);
                    }
                }
            }
        }
    }
}
