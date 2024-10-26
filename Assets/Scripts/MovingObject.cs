using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3 pos1;
    [SerializeField] private Vector3 pos2;

    [SerializeField] private float speed = 5f;

    [SerializeField] bool curve = false;
    [SerializeField] bool random = false;

    private float sinTime;

    private bool direction = true;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.position;

        var parent = transform.parent;
        pos2 += parent.position;

        transform.position = pos1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float t;

        if(curve)
        {
            sinTime += Time.deltaTime * speed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            t = evaluate(sinTime);
        }
        else
        {
            sinTime += Time.deltaTime * speed;
            t = sinTime;
        }

        if (direction)
        {
            transform.position = Vector2.Lerp(transform.position, pos1, t);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, pos2, t);
        }

        if(transform.position == pos1)
        {
            direction= false;
            sinTime = 0;
        }
        else if(transform.position == pos2)
        {
            direction= true;
            sinTime = 0;
        }
    }

    private float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
