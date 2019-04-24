using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorJuego : MonoBehaviour
{

    public GameObject Protagonista;
    public GameObject Monstruo;
    public GameObject padreEscondites;
    public GameObject objetivoMonstruo;
    public GameObject padreBalas;
    public GameObject esconditeActual;
    public Text FinDePartida;
    public Text Victoria;
    public Text tiempo;
    public Text balas;
    public Text vidaMonstruo;

    float tiempoDespertar = 0;
    int tiempoContador;
    float tiempoEsperado;
    bool protagonistaMuerto;
    bool monstruoMuerto;
    int proximaEscena;
    private string nombreTablaPuntuacion;

    // Use this for initialization

    private void Awake()
    {
        for(int i = 0; i < padreBalas.transform.childCount; i++)
        {
            padreBalas.transform.GetChild(i).gameObject.SetActive(false);
        }
        List<int> numerosCogidos = new List<int>();
        int ngenerados = 0;
        while(ngenerados < 5)
        {
            int hijoBala = Random.Range(0, padreBalas.transform.childCount - 1);

            if (!numerosCogidos.Contains(hijoBala))
            {
                padreBalas.transform.GetChild(hijoBala).gameObject.SetActive(true);
                ngenerados++;
                numerosCogidos.Add(hijoBala);
            }
        }

        

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 0)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }
        }

        switch (PlayerPrefs.GetInt("Dificultad"))
        {
            case 0: Monstruo.GetComponent<Monstruo>().indiceVida = 12;
                break;
            case 1: Monstruo.GetComponent<Monstruo>().indiceVida = 18;
                break;
            case 2: Monstruo.GetComponent<Monstruo>().indiceVida = 30;
                break;
        }

        switch (PlayerPrefs.GetInt("Dificultad"))
        {
            case 0:
                nombreTablaPuntuacion = "NombrePosicion";
                break;
            case 1:
                nombreTablaPuntuacion = "NombrePosicion1";
                break;
            case 2:
                nombreTablaPuntuacion = "NombrePosicion2";
                break;
        }

        esconditeActual = null;

    }

    IEnumerator Start()
    {

        yield return new WaitForSeconds(3);
        Monstruo.SetActive(true);
        objetivoMonstruo = Protagonista;
        Monstruo.GetComponent<Monstruo>().objetivo = objetivoMonstruo;
        tiempoEsperado = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        balas.text = "X " + Protagonista.GetComponent<Protagonista>().nbalas;
        vidaMonstruo.text = "" + Monstruo.GetComponent<Monstruo>().indiceVida;
        tiempo.text = "Tiempo: " + tiempoContador;

        if((protagonistaMuerto || monstruoMuerto) && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(proximaEscena);
        }
        if (Protagonista.GetComponent<Protagonista>().isDead)
        {
            Monstruo.GetComponent<Monstruo>().enemigoMuerto = true;
            protagonistaMuerto = true;
            FinDePartida.gameObject.SetActive(true);
            proximaEscena = 0;
        }
        else if (Monstruo.GetComponent<Monstruo>().monstruoMuerto)
        {
            monstruoMuerto = true;
            Victoria.gameObject.SetActive(true);

            if (nuevoRecord())
            {
                proximaEscena = 3;
                PlayerPrefs.SetInt("PuntuacionJugador", tiempoContador);
            }
            else
            {
                proximaEscena = 0;
            }
        }

        else if (Protagonista.GetComponent<Protagonista>().invisible && objetivoMonstruo != null && objetivoMonstruo.tag == "Player")
        {
            GameObject[] escondites = new GameObject[4];
            int nObjetos = 0;

            for (int i = 0; i < padreEscondites.transform.childCount; i++)
            {
                Vector2 posEscondite = padreEscondites.transform.GetChild(i).position;

                if (distancia(posEscondite, Protagonista.transform.position) < 11 && padreEscondites.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    escondites[nObjetos] = padreEscondites.transform.GetChild(i).gameObject;
                    if(esconditeActual == null || distancia(posEscondite,Protagonista.transform.position) < distancia(esconditeActual.transform.position, Protagonista.transform.position))
                    {
                        esconditeActual = escondites[nObjetos];
                    }
                    nObjetos++;
                }
            }

            objetivoMonstruo = escondites[Random.Range(0, nObjetos)];
            Monstruo.GetComponent<Monstruo>().objetivo = objetivoMonstruo;
        }

        else if (!Protagonista.GetComponent<Protagonista>().invisible && objetivoMonstruo != null && !Monstruo.GetComponent<Monstruo>().dormir && objetivoMonstruo.tag != "Player")
        {
            objetivoMonstruo = Protagonista;
            Monstruo.GetComponent<Monstruo>().objetivo = objetivoMonstruo;
        }

        else if (objetivoMonstruo != null && (objetivoMonstruo.tag == "Hidding" || objetivoMonstruo.tag == "HiddingUp") && !objetivoMonstruo.activeInHierarchy)
        {
            Monstruo.GetComponent<Monstruo>().dormir = true;
            if(Monstruo.GetComponent<Monstruo>().velocidad > 4)
            {
                Monstruo.GetComponent<Monstruo>().velocidad = 4;
            }

            for (int i = 0; i < padreBalas.transform.childCount; i++)
            {
                padreBalas.transform.GetChild(i).gameObject.SetActive(false);
            }

            List<int> numerosCogidos = new List<int>();
            int ngenerados = 0;

            while (ngenerados < 5)
            {
                int hijoBala = Random.Range(0, padreBalas.transform.childCount - 1);

                if (!numerosCogidos.Contains(hijoBala))
                {
                    padreBalas.transform.GetChild(hijoBala).gameObject.SetActive(true);
                    ngenerados++;
                    numerosCogidos.Add(hijoBala);
                }
            }

            objetivoMonstruo = null;
        }

        else if (!Protagonista.GetComponent<Protagonista>().invisible && Monstruo.GetComponent<Monstruo>().dormir && tiempoDespertar < 2)
        {
            tiempoDespertar += Time.deltaTime;
        }

        else if (Protagonista.GetComponent<Protagonista>().invisible && Monstruo.GetComponent<Monstruo>().dormir)
        {
            tiempoDespertar = 0;
        }

        else if (!Protagonista.GetComponent<Protagonista>().invisible && Monstruo.GetComponent<Monstruo>().dormir)
        {
            Monstruo.GetComponent<Monstruo>().despertar = true;
            objetivoMonstruo = Protagonista;
            Monstruo.GetComponent<Monstruo>().objetivo = objetivoMonstruo;
        }
        if(esconditeActual != null && !esconditeActual.activeInHierarchy && Protagonista.GetComponent<Protagonista>().invisible)
        {
            Protagonista.GetComponent<Protagonista>().muere();
        }

        if(Time.time > tiempoEsperado && !Protagonista.GetComponent<Protagonista>().isDead && !Monstruo.GetComponent<Monstruo>().monstruoMuerto)
        {
            tiempoContador++;
            tiempoEsperado = Time.time + 1;
        }

    }

    float distancia(Vector2 v1, Vector2 v2)
    {

        float x1 = v1.x, x2 = v2.x;
        float y1 = v1.y, y2 = v2.y;

        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
    }

    bool nuevoRecord()
    {
        bool record = false;
        int i = 0;

        while (!record && i < 5)
        {
            if (PlayerPrefs.GetInt(nombreTablaPuntuacion + i) > tiempoContador || PlayerPrefs.GetInt(nombreTablaPuntuacion + i) == 0)
            {
                record = true;
            }
            else
            {
                i++;
            }
        }

        return record;
    }

}
