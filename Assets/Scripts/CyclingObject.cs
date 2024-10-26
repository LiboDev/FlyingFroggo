using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclingObject : MonoBehaviour
{
    [SerializeField] private bool randomization;

    private Vector2 pos1;
    private Vector2 pos2;

    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        var parent = transform.parent;
        pos1 = new Vector3(-11,transform.position.y,0);
        pos2 = new Vector3(11,transform.position.y,0);
        transform.position += new Vector3(Random.Range(0, 19f), 0, 0);

        if (randomization)
        {
            speed += Random.Range(-1f, 1f);
        }

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        while (true)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;

            if(transform.position.x > pos2.x)
            {
                if (randomization)
                {
                    spriteRenderer.enabled = false;

                    speed += Random.Range(-1f, 1f);
                    spriteRenderer.enabled = true;
                }

                yield return new WaitForSeconds(Random.Range(0f, 3f));
                transform.position = pos1;
            }
            yield return null;
        }
    }
}
