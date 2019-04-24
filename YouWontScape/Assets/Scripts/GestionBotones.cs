using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionBotones : MonoBehaviour {

    public InputField nombreUsuario;
    public Text Error;

    string nombreTablaPuntuacion;
    string nombreTablaNombres;


    private void Awake()
    {
        switch (PlayerPrefs.GetInt("Dificultad"))
        {
            case 0:
                nombreTablaPuntuacion = "NombrePosicion";
                nombreTablaNombres = "PuntuacionPosicion";
                break;
            case 1:
                nombreTablaPuntuacion = "NombrePosicion1";
                nombreTablaNombres = "PuntuacionPosicion1";
                break;
            case 2:
                nombreTablaPuntuacion = "NombrePosicion2";
                nombreTablaNombres = "PuntuacionPosicion2";
                break;
        }
    }

    public void clickEnBotones(int boton)
    {
        if(boton == 2 && nombreUsuario.text == "")
        {
            Error.gameObject.SetActive(true);
        }
        else
        {
            if(boton == 2)
            {
                nuevoRecord();
            }
            SceneManager.LoadScene(0);
        }
    }

    void nuevoRecord()
    {
        int i = 0;
        bool encontrado = false;

        while(!encontrado && i < 5)
        {
            if (PlayerPrefs.GetInt(nombreTablaPuntuacion + i) > PlayerPrefs.GetInt("PuntuacionJugador") || PlayerPrefs.GetInt(nombreTablaPuntuacion + i) == 0)
            {
                encontrado = true;
            }
            else
            {
                i++;
            }
        }

        if((PlayerPrefs.GetString(nombreTablaNombres+i) != nombreUsuario.text))
        {
            int j = 4;

            while (j >= i)
            {
                if (PlayerPrefs.GetInt(nombreTablaPuntuacion + j) != 0 && (j + 1) != 5)
                {
                    PlayerPrefs.SetInt(nombreTablaPuntuacion + (j + 1), PlayerPrefs.GetInt(nombreTablaPuntuacion + j));
                    PlayerPrefs.SetString(nombreTablaNombres + (j + 1), PlayerPrefs.GetString(nombreTablaNombres + j));
                }
                j--;
            }
        }
        
        PlayerPrefs.SetInt(nombreTablaPuntuacion + i, PlayerPrefs.GetInt("PuntuacionJugador"));
        PlayerPrefs.SetString(nombreTablaNombres + i, nombreUsuario.text);


    }

}
