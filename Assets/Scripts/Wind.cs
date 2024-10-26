using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private float rotation = 0;
    private Rigidbody2D playerrb;

    // Start is called before the first frame update
    void Start()
    {
        playerrb = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotation += Random.Range(-1, 1);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        playerrb.velocity += new Vector2(Mathf.Cos(Mathf.Deg2Rad*rotation), Mathf.Sin(Mathf.Deg2Rad*rotation)) * 0.1f;
    }
}
