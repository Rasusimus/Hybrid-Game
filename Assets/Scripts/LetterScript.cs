using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterScript : MonoBehaviour
{
    private bool clicked = false;
    private bool escapeKeyPressed = false;
    private bool isInFront = false;

    private float growthDuration = 1f;
    private float elapsedTime = 0f;

    [SerializeField] private float initialSize;
    [SerializeField] private float maxSize;

    private Vector2 initialScale;
    private Vector2 maxScale;

    [SerializeField] private Transform arrow;
    [SerializeField] private Transform paper;
    private SpriteRenderer arrow_sprite;
    private SpriteRenderer letter_sprite;
    private SpriteRenderer paper_sprite;
    private int order;

    [SerializeField] private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        arrow_sprite = arrow.GetComponent<SpriteRenderer>();
        letter_sprite = GetComponent<SpriteRenderer>();
        paper_sprite = paper.GetComponent<SpriteRenderer>();
        order = letter_sprite.sortingOrder;

        rb = player.GetComponent<Rigidbody2D>();

        initialScale = new Vector2(initialSize, initialSize);
        maxScale = new Vector2(maxSize, maxSize);
    }

    private void OnMouseDown()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic && !escapeKeyPressed && !isInFront)
        {
           clicked = true;
        }
    }

    void Update()
    {
        if (clicked)
        {
            arrow_sprite.sortingOrder = 0;
            letter_sprite.sortingOrder = 0;
            paper_sprite.sortingOrder = 11;
            rb.bodyType = RigidbodyType2D.Static;

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / growthDuration;

            t = Mathf.Clamp01(t);

            paper.localScale = Vector2.Lerp(initialScale, maxScale, t);

            if (t >= 1)
            {
                clicked = false;
                isInFront = true;
                elapsedTime = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeKeyPressed = true;
        }
        if (escapeKeyPressed)
        {
            letter_sprite.sortingOrder = order;

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / growthDuration;

            t = Mathf.Clamp01(t);

            paper.localScale = Vector2.Lerp(maxScale, initialScale, t);

            if (t >= 1)
            {
                escapeKeyPressed = false;
                isInFront = false;
                elapsedTime = 0;
                paper_sprite.sortingOrder = 0;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
