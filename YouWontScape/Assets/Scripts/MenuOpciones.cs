using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour {

    public AudioMixer audioMixer;
    public AudioMixer effectsMixer;
    public Slider sliderVolumen;
    public Slider sliderEfectos;
    public Dropdown persianaDificultad;

    private void Awake()
    {
        sliderVolumen.value = PlayerPrefs.GetFloat("MusicVolume");
        sliderEfectos.value = PlayerPrefs.GetFloat("EffectsVolume");
        persianaDificultad.value = PlayerPrefs.GetInt("Dificultad");
    }

    public void setVolume(float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
    }

    public void setEffects(float effects)
    {
        effectsMixer.SetFloat("volumenEfectos", effects);
    }

    private void OnDestroy()
    {
        float volumen;
        float efectos;
        audioMixer.GetFloat("volumen", out volumen);
        effectsMixer.GetFloat("volumenEfectos", out efectos);
        PlayerPrefs.SetFloat("MusicVolume", volumen);
        PlayerPrefs.SetFloat("EffectsVolume", efectos);
        PlayerPrefs.SetInt("Dificultad", persianaDificultad.value);
    }

}
