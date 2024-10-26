using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody2D rb;
    private ObjectEffects objectEffects;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity += new Vector2(Random.Range(-10,10), Random.Range(-1,1));

        objectEffects = GetComponent<ObjectEffects>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //triggers audio, particle and tween effect
        ObjectEffects script = other.gameObject.GetComponent<ObjectEffects>();
        if (script != null)
        {
            script.Play();
        }
        objectEffects.Play();

        if (other.gameObject.name == "Chase")
        {
            Destroy(gameObject);
        }
    }
}
