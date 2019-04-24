using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonInicio : MonoBehaviour {

    public GameObject canvasActual;
	
    public void click(int escena)
    {
        if(escena >= 0)
        {
            SceneManager.LoadScene(escena);
        }
        else
        {
            Application.Quit();
        }
    }

    public void cambiarCanvas(GameObject nombre)
    {
        nombre.SetActive(true);
        canvasActual.SetActive(false);
    }
}
