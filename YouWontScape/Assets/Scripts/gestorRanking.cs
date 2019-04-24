using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gestorRanking : MonoBehaviour
{
    public GameObject panelesNombre;
    public GameObject panelesPuntos;
    string nombreTablaPuntuacion;
    string nombreTablaNombres;
    public int dificultadRanking;

    private void Awake()
    {
        switch (dificultadRanking)
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

        for (int i = 0; i < 5; i++)
        {
            panelesNombre.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(nombreTablaNombres + i);
            panelesPuntos.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "" + PlayerPrefs.GetInt(nombreTablaPuntuacion + i);
        }
    }

    public void reiniciar()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetString(nombreTablaNombres + i, "");
            PlayerPrefs.SetInt(nombreTablaPuntuacion + i, 0);
            panelesNombre.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
            panelesPuntos.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0";
        }
    }


}
