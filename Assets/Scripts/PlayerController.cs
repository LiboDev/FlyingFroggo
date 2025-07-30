using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using RDG;

public class PlayerController : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject cam;
    [SerializeField] private Transform tounge;
    private Transform shadow;

    [SerializeField] private DynamicJoystick floatingJoystick;
    [SerializeField] private ButtonHeld abilityButton;
    [SerializeField] private GameObject tracer;
    private Transform tracerTransform;
    [SerializeField] private RectTransform hearts;
    [SerializeField] private Slider slider;

    //object
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private List<Sprite> sprites;

    //stats
    [SerializeField] private float grappelForce;

    [SerializeField] private int maxHealth;
    private int health = 0;

    [SerializeField] private float abilityCooldown;
    [SerializeField] private bool canFart = false;
    [SerializeField] private bool canHeal = false;
    [SerializeField] private bool canShield = false;
    [SerializeField] private bool canTeleport = false;

    //tracking
    private Vector2 playerPos;
    private Vector2 mousePos;

    private Vector2 toungeDirection;
    private Vector2 mouseDirection;
    private float angle;
    private float strafe;

    private bool hooked = false;
    private bool invincible = false;

    private float invincibilityTimer = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        shadow = transform.Find("Shadow");

        tracerTransform = tracer.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(abilityCooldown > 0)
        {
            Invoke("Abilities", 0.1f);
        }
        else
        {
            abilityButton.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
        }
    }

    void Abilities()
    {
        //special abilities
        if (canFart)
        {
            StartCoroutine(Fart());
        }
        else if (canShield)
        {
            StartCoroutine(Shield());
        }
        else if (canHeal)
        {
            StartCoroutine(Regenerate());
        }
        else if (canTeleport)
        {
            StartCoroutine(Teleport());
        }

        StartCoroutine(AbilityCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        //update player rotation
        if (hooked)
        {
            angle = Mathf.Atan2(toungeDirection.y, toungeDirection.x) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, 0, angle - 90f), Time.deltaTime * 20f);

        //update tracer position and angle
        tracerTransform.position = transform.position;
        var joystickVector = Vector3.up * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        if (joystickVector.magnitude == 0)
        {
            tracer.SetActive(false);
        }
        else
        {
            tracer.SetActive(true);
        }
        var tracerAngle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        tracerTransform.rotation = Quaternion.Slerp(tracerTransform.rotation, Quaternion.Euler(0, 0, tracerAngle - 90f), Time.deltaTime * 20f);
        


        //invincibility timer
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (invincible)
        {
            invincibilityTimer = 0;
            invincible = false;
        }

        //shadow follow
        shadow.position = transform.position - Vector3.up / 5;

        //menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }

    //boost
    private IEnumerator Fart()
    {
        yield return new WaitForSeconds(abilityCooldown);

        while (true)
        {
            if (abilityButton.held && hooked == false)
            {
                var dir = mouseDirection.normalized;
                rb.velocity += dir * 20;

                StartCoroutine(AbilityCooldown());
                yield return new WaitForSeconds(abilityCooldown);
            }
            yield return null;
        }
    }

    //Invincibility
    private IEnumerator Shield()
    {
        yield return new WaitForSeconds(abilityCooldown);

        while (true)
        {
            if (abilityButton.held && invincible == false)
            {
                invincible= true;
                invincibilityTimer += 5f;

                var dir = mouseDirection.normalized;
                rb.velocity += dir * 20;

                StartCoroutine(AbilityCooldown());
                yield return new WaitForSeconds(abilityCooldown);
            }
            yield return null;
        }
    }

    //heal hp overtime
    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(abilityCooldown);

        while (true)
        {
            if (abilityButton.held && hooked == false)
            {
                ChangeHP(1);

                StartCoroutine(AbilityCooldown());
                yield return new WaitForSeconds(abilityCooldown);
            }

            yield return null;
        }
    }

    //heal hp overtime
    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(abilityCooldown);

        while (true)
        {
            if (abilityButton.held && hooked == false)
            {
                Vector3 dir = mouseDirection.normalized * 5;
                Vector2 newPos = transform.position + dir;

                if(newPos.x < -9)
                {
                    newPos -= new Vector2(9+newPos.x, 0);
                }
                else if (newPos.x > 9)
                {
                    newPos -= new Vector2(newPos.x-9, 0);
                }
                transform.position = newPos;

                StartCoroutine(AbilityCooldown());
                yield return new WaitForSeconds(abilityCooldown);
            }

            yield return null;
        }
    }

    //cooldown
    private IEnumerator AbilityCooldown()
    {
        slider.value = 0;

        while (true)
        {
            if (slider.value < 1)
            {
                slider.value += Time.deltaTime / abilityCooldown;
            }
            else
            {
                slider.value = 1;
                break;
            }
            yield return null;
        }
    }

    void FixedUpdate()
    {
        //rotation direction point towards mousepos

        /*        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                playerPos = transform.position;
                mouseDirection = mousePos - playerPos;*/

        //direction for joystick
        var temp = mouseDirection;
        mouseDirection = Vector3.up * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        if(mouseDirection.magnitude == 0)
        {
            mouseDirection= temp;
        }

        if (hooked)
        {
            //rotation direction point towards grappled object
            toungeDirection = tounge.position - transform.position;

            rb.velocity += toungeDirection * grappelForce * Time.deltaTime;
        }
    }

    public void Shoot()
    {
        //Vibration.VibratePredefined(0);

        var dir = mouseDirection.normalized;
        rb.velocity -= dir * grappelForce / 2;
    }
    public void Hooked()
    {
        Vibration.VibratePredefined(0);
        hooked = true;
    }
    public void UnHooked()
    {
        //Vibration.VibratePredefined(0);
        var dir = mouseDirection.normalized;
        rb.velocity += dir * grappelForce * 2;
        hooked = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Harmful")
        {
            Vibration.VibratePredefined(2);
            ChangeHP(-1);
        }
        else
        {
            Vibration.VibratePredefined(0);
        }

        //triggers audio, particle and tween effect
        ObjectEffects script = other.gameObject.GetComponent<ObjectEffects>();
        if (script != null)
        {
            script.Play();
        }
    }

    public void ChangeHP(int change)
    {
        if(invincible == false)
        {
            var newHealth = health + change;

            if (newHealth < 0)
            {
                health = 0;
            }
            else if (newHealth > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = newHealth;
            }
        }

        if (change < 0)
        {
            invincible = true;
            invincibilityTimer += 1f;
        }
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
        //enabled = false;
    }

    //set character stats and abilities from character shop
    public void SetCharacter(Characters character)
    {
        spriteRenderer.sprite = character.characterSprite;

        health = character.health;

        grappelForce = character.grappelForce;

        abilityCooldown = character.abilityCooldown;

        canFart = character.canFart;
        canHeal = character.canHeal;
        canShield = character.canShield;
        canTeleport = character.canTeleport;

        rb.gravityScale = character.gravityScale;

        maxHealth = character.health;
        health = maxHealth;
        ChangeHP(health);
    }
}
