using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonista : MonoBehaviour
{

    public Camera camaraPrincipal;
    public float velocidad = 0.4f;
    public bool invisible = false;
    public bool isDead = false;
    public float fireRate;

    public GameObject shoot;
    public Transform shootSpawn;
    public AudioSource pisada;

    private float nextFire;

    Animator anim;
    SpriteRenderer sr;

    float profundidadCamara;
    bool corriendo = false;
    bool disparando;
    float direccionActual = 1;
    public int nbalas = 6;

    Vector3 originalColliderSize;
    Vector3 originalColliderOffset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        profundidadCamara = camaraPrincipal.transform.position.z;
        originalColliderSize = GetComponent<BoxCollider2D>().size;
        originalColliderOffset = GetComponent<BoxCollider2D>().offset;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            float direccionX = Input.GetAxisRaw("Horizontal");
            float direccionY = Input.GetAxisRaw("Vertical");

            if ((direccionX != 0 || direccionY != 0) && !corriendo)
            {
                corriendo = true;
                anim.SetTrigger("Run");
            }
            else if (direccionX == 0 && direccionY == 0 && corriendo)
            {
                corriendo = false;
                anim.SetTrigger("StopRunning");
            }
            if(corriendo && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.SetTrigger("Run");
            }
            if (Input.GetButtonDown("Fire1") && nbalas > 0)
            {

                corriendo = false;
                disparando = true;

                anim.SetTrigger("Shoot");
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                disparando = false;
                anim.SetTrigger("StopShooting");
            }
            if (Input.GetButton("Fire1") && Time.time > nextFire && nbalas > 0)
            {
                nbalas--;
                nextFire = Time.time + fireRate;
                Instantiate(shoot, shootSpawn.position, shootSpawn.rotation).GetComponent<Disparo>().direccion = direccionActual;
            }

            if (direccionX != direccionActual && direccionX != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                direccionActual = direccionX;
                shoot.GetComponent<Disparo>().direccion = direccionX;
            }

            if(corriendo && (sr.sprite.name == "Run_003" || sr.sprite.name == "Run_010"))
            {
                pisada.Play();
            }

            if (!disparando)
            {
                float posicionX = transform.position.x + (direccionX * velocidad * Time.deltaTime);
                float posicionY = transform.position.y + (direccionY * velocidad * Time.deltaTime);
                transform.position = new Vector2(posicionX, posicionY);
                Camera.main.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, profundidadCamara);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hidding" || collision.tag == "HiddingUp")
        {
            Color tmp = sr.color;
            if (tmp.a == 1)
            {
                tmp.a /= 5;
            }
            sr.color = tmp;

            //A continuación cambiamos el tamaño del collider para que quede dentro del trigger
            Vector3 newSize = originalColliderSize;
            Vector3 newOffset = originalColliderOffset;
            newSize.y = 3f;
            if (collision.tag == "Hidding")
            {
                newOffset.y = -1.1f;
            }
            else
            {
                newOffset.y = 0.5f;
            }
            GetComponent<BoxCollider2D>().size = newSize;
            GetComponent<BoxCollider2D>().offset = newOffset;

            invisible = true;
        }
        else if (collision.tag == "Monster" && !collision.gameObject.GetComponent<Monstruo>().dormir && !collision.gameObject.GetComponent<Monstruo>().monstruoMuerto)
        {
            muere();
        }
        else if(collision.tag == "Bullets" && nbalas < 6)
        {
            nbalas = 6;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hidding" || collision.tag == "HiddingUp")
        {
            Color tmp = sr.color;
            tmp.a *= 5;
            sr.color = tmp;

            GetComponent<BoxCollider2D>().size = originalColliderSize;
            GetComponent<BoxCollider2D>().offset = originalColliderOffset;
            invisible = false;
        }
    }

    public void muere()
    {
        Color tmp = sr.color;
        tmp.a = 255;
        sr.color = tmp;

        invisible = false;
        isDead = true;
        anim.SetTrigger("Die");
    }

}
