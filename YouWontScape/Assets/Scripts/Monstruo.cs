using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monstruo : MonoBehaviour {
    
    public GameObject objetivo;
    public float velocidad = 8f;
    public float indiceVida = 30;
    public bool enemigoMuerto = false;
    public bool dormir = false;
    public bool despertar = false;
    public bool monstruoMuerto = false;

    List<Posicion> CaminoASeguir;
    Animator anim;
    Vector2 posObjetivo;
    Vector2 direccionObjetivo;
    Posicion destinoActual;
    bool enDestinoActual = false;
    float danoAcumulado;
    float tiempoParado;
    float tiempoAturdimiento;
    float tiempoSinDano;
    bool aturdido = false;


	// Use this for initialization
	void Start () {
        posObjetivo = objetivo.transform.position;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !enemigoMuerto && !dormir && !monstruoMuerto && Time.time > tiempoParado && Time.time > tiempoAturdimiento)
        {
            if ((!enObjetivo() && (CaminoASeguir == null || CaminoASeguir.Count == 0)) || objetivoSeHaMovido())
            {
                posObjetivo = objetivo.transform.position;
                int x=0, y=0;
                if (posObjetivo.x < transform.position.x)
                {
                    x = -1;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else if(posObjetivo.x > transform.position.x)
                {
                    x = 1;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                if(posObjetivo.y < transform.position.y)
                {
                    y = -1;
                }
                else if(posObjetivo.y > transform.position.y)
                {
                    y = 1;
                }

                direccionObjetivo = new Vector2(x, y);

                CaminoASeguir = new List<Posicion>();
                AEstrella();
                destinoActual = CaminoASeguir[0];
                CaminoASeguir.RemoveAt(0);
                enDestinoActual = false;
            }

            if (enDestinoActual)
            {
                destinoActual = CaminoASeguir[0];
                CaminoASeguir.RemoveAt(0);
                enDestinoActual = false;
            }

            float X = transform.position.x + (destinoActual.direccionPosicion.x * velocidad * Time.deltaTime);
            float Y = transform.position.y + (destinoActual.direccionPosicion.y * velocidad * Time.deltaTime);

            Vector2 destino = new Vector2(X, Y);

            if(distancia(transform.position, destino) > distancia(transform.position, destinoActual.getPosicion()))
            {
                destino = destinoActual.getPosicion();
                enDestinoActual = true;
            }

            transform.position = destino;

        }

        else if (enemigoMuerto || dormir || monstruoMuerto)
        {
            anim.SetTrigger("Die");
        }
        if (despertar && !monstruoMuerto)
        {
            anim.ResetTrigger("Die");
            anim.SetTrigger("Reborn");
            dormir = false;
            despertar = false;
        }

        if(!monstruoMuerto && indiceVida <= 0)
        {
            monstruoMuerto = true;
        }

        if (danoAcumulado != 0 && 30 / danoAcumulado == 5)
        {
            velocidad += 2;
            danoAcumulado = 0;
            tiempoAturdimiento = Time.time + 4f;
            aturdido = true;
        }
        else if(Time.time > tiempoSinDano && danoAcumulado < 5 && !aturdido)
        {
            indiceVida += danoAcumulado;
            danoAcumulado = 0;
        }

        if(Time.time > tiempoAturdimiento && aturdido)
        {
            aturdido = false;
        }

    }

    void AEstrella()
    {
        List<KeyValuePair<Posicion, float>> abiertos = new List<KeyValuePair<Posicion, float>>();
        List<Posicion> cerrados = new List<Posicion>();

        Posicion inicial = new Posicion(transform.position, objetivo.transform.position, Vector2.zero, direccionObjetivo, 0, velocidad, null);

        abiertos.Add(new KeyValuePair<Posicion, float>(inicial, inicial.coste));
        Posicion actual = inicial;

        while(actual.distancia(objetivo.transform.position) != 0 && abiertos.Count() > 0)
        {
            abiertos.RemoveAt(0);
            cerrados.Add(actual);
            List<Posicion> hijos = actual.generaHijos();
            tratarRepetidos(ref hijos, ref abiertos, ref cerrados);

            actual = abiertos[0].Key;
        }

        while(actual.padre != null)
        {
            CaminoASeguir.Insert(0, actual);
            actual = actual.padre;
        }

    }

    

    void tratarRepetidos(ref List<Posicion> hijos, ref List<KeyValuePair<Posicion,float>> abiertos, ref List<Posicion> cerrados)
    {
        List<Posicion> hijosNoRepetidos = new List<Posicion>();

        List<Posicion> abiertosSoloPosicion = new List<Posicion>();

        foreach (KeyValuePair<Posicion,float> p in abiertos)
        {
            abiertosSoloPosicion.Add(p.Key);
        }

        while(hijos.Count > 0)
        {
            Posicion actual = hijos[0];
            hijos.RemoveAt(0);

            if (cerrados.Contains(actual))
            {
                int indice = cerrados.IndexOf(actual);
                Posicion cerradoRepetido = cerrados[indice];

                if(actual.coste < cerradoRepetido.coste)
                {
                    cerrados.RemoveAt(indice);
                }
            }
            if(abiertosSoloPosicion.Contains(actual) && !cerrados.Contains(actual))
            {
                int indice = abiertosSoloPosicion.IndexOf(actual);
                Posicion abiertoRepetido = abiertosSoloPosicion[indice];

                if(actual.coste < abiertoRepetido.coste)
                {
                    abiertos[indice] = new KeyValuePair<Posicion, float>(actual, actual.coste);
                    abiertos = abiertos.OrderBy(pos => pos.Value).ToList();
                }
            }
            else if(!abiertosSoloPosicion.Contains(actual) && !cerrados.Contains(actual))
            {
                hijosNoRepetidos.Add(actual);
            }
        }

        foreach(Posicion hijo in hijosNoRepetidos)
        {
            abiertos.Add(new KeyValuePair<Posicion, float>(hijo, hijo.coste));
        }

        abiertos = abiertos.OrderBy(pos => pos.Value).ToList();

    }

    bool enObjetivo()
    {
        if(transform.position.x == objetivo.transform.position.x && transform.position.y == objetivo.transform.position.y)
        {
            return true;
        }
        return false;
    }

    bool objetivoSeHaMovido()
    {
        return (objetivo.transform.position.x != posObjetivo.x || objetivo.transform.position.y != posObjetivo.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Hidding" || collision.tag == "HiddingUp") && distancia(collision.transform.position, posObjetivo) == 0)
        {
            collision.gameObject.SetActive(false);
        }
        else if(collision.tag == "Shoot" && !monstruoMuerto)
        {
            tiempoParado = Time.time + 2f;
            tiempoSinDano = Time.time + 10f;
            Destroy(collision.gameObject);
            indiceVida--;
            danoAcumulado++;
        }

    }

    float distancia(Vector2 v1, Vector2 v2)
    {

        float x1 = v1.x, x2 = v2.x;
        float y1 = v1.y, y2 = v2.y;

        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
    }
}
