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

    [SerializeField] private ChunkSpawner chunkSpawner;

    [SerializeField] private GameObject fartParticles;
    [SerializeField] private GameObject teleportParticles;
    [SerializeField] private GameObject shieldObject;

    [SerializeField] private GameObject dummy;


    //object
    private Rigidbody2D rb;
    private Transform spriteTransform;
    private SpriteRenderer spriteRenderer;
    private Animation spriteAnimation;

    [SerializeField] private List<Sprite> sprites;

    //stats
    public float range = 10f;
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

        spriteTransform = transform.GetChild(3);
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        spriteAnimation = spriteTransform.GetComponent<Animation>();

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
                PlaySound.instance.PlaySFX("Fart", 1f, 0.05f);

                Instantiate(fartParticles, transform.position, transform.rotation);

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
                PlaySound.instance.PlaySFX("Shield", 1f, 0.05f);

                GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
                shield.transform.SetParent(spriteTransform);

                StartCoroutine(DestroyObject(shield, 5f));

                invincible = true;
                invincibilityTimer += 5f;

                StartCoroutine(AbilityCooldown());
                yield return new WaitForSeconds(abilityCooldown);
            }
            yield return null;
        }
    }

    private IEnumerator DestroyObject(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
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
                PlaySound.instance.PlaySFX("Teleport", 1f, 0.05f);

                Instantiate(teleportParticles, transform.position, transform.rotation);

                Vector3 dir = mouseDirection.normalized * 10;
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


                Instantiate(teleportParticles, transform.position, transform.rotation);

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
                PlaySound.instance.PlaySFX("AbilityRefresh", 1f, 0.05f);

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

        PlaySound.instance.PlaySFX("Shoot", 1f, 0.05f);
    }
    public void Hooked()
    {
        spriteAnimation.Play();
        Vibration.VibratePredefined(0);
        hooked = true;

        PlaySound.instance.PlaySFX("TongueHit", 1f, 0.05f);
    }
    public void UnHooked()
    {
        //Vibration.VibratePredefined(0);
        var dir = mouseDirection.normalized;
        rb.velocity += dir * grappelForce * 2;
        hooked = false;

        PlaySound.instance.PlaySFX("HookRelease", 1f, 0.05f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //spriteTransform.GetComponent<Animation>().Stop();
        spriteAnimation.Play();

        if(other.gameObject.tag == "Harmful")
        {
            Vibration.VibratePredefined(2);
            GameOver();

            //ChangeHP(-1);

            PlaySound.instance.PlaySFX("DamageTaken", 1f, 0.01f);
        }
        else
        {
            Vibration.VibratePredefined(0);

            PlaySound.instance.PlaySFX("FrogHit", 1f, 0.01f);
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
        PlaySound.instance.PlaySFX("GameOver", 1f);

        chunkSpawner.GameOver();

        GameObject dummyObject = Instantiate(dummy, transform.position, transform.rotation);
        dummyObject.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        Rigidbody2D rb = dummyObject.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(-5f, 5f), 5f));
        rb.AddTorque(Random.Range(-5f, 5f));

        tracer.SetActive(false);

        gameObject.SetActive(false);
    }

    //set character stats and abilities from character shop
    public void SetCharacter(Characters character)
    {
        spriteRenderer.sprite = character.characterSprite;

        health = character.health;

        grappelForce = character.grappelForce;
        range = character.range;

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