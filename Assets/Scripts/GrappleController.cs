using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject cam;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform tracerTransform;
    [SerializeField] private DynamicJoystick floatingJoystick;
    [SerializeField] private ButtonHeld shootButton;

    //object
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;

    //stats
    [SerializeField] private float range;
    [SerializeField] private float speed;

    //tracking
    private Vector2 mousePos;
    private Vector2 startPos;
    private bool returning = false;
    private bool canShoot = true;
    private Vector2 joystickVector;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;

        range = playerController.range;
        tracerTransform.localScale = new Vector3(0.5f, range + 1, 1);
        tracerTransform.position = new Vector3(0, range / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //set tounge body points
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, transform.position);

        if (!returning)
        {
            if (shootButton.held && transform.parent == player && canShoot)
            {
                Shoot();
                canShoot = false;
                Invoke("Reload", 0.5f);
            }
            else if ((shootButton.released && transform.parent != player)   ||  (Vector2.Distance(transform.position, player.position) > range && transform.parent == null))
            {
                StartCoroutine(Return());
            }
        }


        if(Vector2.Distance(transform.position, player.transform.position) < 0.5f && returning)
        {
            Parent(player);
            returning = false;
        }

        //joystick input only change if not zero
        var temp = joystickVector;
        joystickVector = Vector3.up * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        if (joystickVector.magnitude == 0)
        {
            joystickVector = temp;
        }
    }

    private void Reload()
    {
        canShoot = true;

        PlaySound.instance.PlaySFX("FrogGulp", 1f, 0.05f);
    }

    private void Shoot()
    {
        playerController.Shoot();

        transform.parent = null;
        returning = false;

        //points tounge towards mouse
        /*        startPos = transform.position;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var direction = mousePos - startPos;*/

        //joystick direction
        var angle = Mathf.Atan2(joystickVector.y, joystickVector.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        //shoot tounge forward
        rb.velocity = transform.up * speed;
    }

    private IEnumerator Return()
    {
        if(transform.parent != null)
        {
            playerController.UnHooked();
            transform.localScale = new Vector3(1,1,1);
        }


        transform.parent = null;
        returning = true;

        //shoot tounge back
        while (returning)
        {
            //points tounge towards player
            Vector2 currentPos = transform.position;
            Vector2 playerPos = player.position;
            var direction = playerPos - currentPos;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            rb.velocity = transform.up * speed * Mathf.Min(Vector2.Distance(transform.position,player.position),2f);
            yield return null;
        }
        
    }

    private void Parent(Transform obj)
    {
        rb.velocity = Vector2.zero;
        transform.position = obj.position;
        transform.parent = obj;
        rb.isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Ungrappleable" && other.gameObject.tag != "Player" && transform.parent == null && shootButton.held)
        {
            Parent(other.gameObject.transform);
            playerController.Hooked();
            returning = false;
        }

        if(other.gameObject.tag == "Ungrappleable" && !returning && transform.parent == null)
        {
            StartCoroutine(Return());
        }

        //triggers audio, particle and tween effect
        ObjectEffects script = other.gameObject.GetComponent<ObjectEffects>();
        if(script != null)
        {
            script.Play();
        }
    }
}
