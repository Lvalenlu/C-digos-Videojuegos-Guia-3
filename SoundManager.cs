using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource Aud;

    public AudioClip[] Clips;

    public string[] Names;

    public TextMeshProUGUI NameLabel;

    public Slider MusicSelector, MusicSlider, SfxSlider;

    public Canvas OptionsCanvas;

    public AudioMixer Mixer;

    // Start is called before the first frame update
    void Start()
    {

        Aud = GetComponent<AudioSource>();
        Aud.clip = Clips[0];
        NameLabel.SetText(Names[0]);//Damos un nombre
        Aud.loop = true;
        Aud.Play();//Suena apenas inicia Play Awake
        OptionsCanvas.enabled = false;//No se muestra
        GetMusicLevel();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){

            OptionsCanvas.enabled = true; 
            Time.timeScale = 0;//Detiene el conteo temporal cuando la escala de tiempo es igual a cero
        }
        //Condicional, variable Pitch para que música se acelere, modificar valor de Pitch
        
    }

    public void MusicSelection(){

        Aud.clip = Clips[(int)MusicSelector.value];
        NameLabel.SetText(Names[(int)MusicSelector.value]);
        Aud.loop = true;
        Aud.Play();
        //Actualizar el Aud para que reproduzca
    }

    public void OptionsClose(){
        OptionsCanvas.enabled = false;
        Time.timeScale = 1;//Cuando es uno la escala de tiempo de mantiene natural

    }

    public void SetMusicLevel(){
        Mixer.SetFloat("MusicLevel", MusicSlider.value);//Actualizando valor de la música con respecto a la posición del slider
    }
    public void GetMusicLevel(){
        float Level;
        Mixer.GetFloat("MusicLevel", out Level);//En nivel del slider se cambie con respecto a la música automáticamente
        MusicSlider.value = Level;
    }

    public void SetSfxLevel(){
        Mixer.SetFloat("SfxLevel", SfxSlider.value);
    }
    public void GetSfxLevel(){
        float Level;
        Mixer.GetFloat("SfxLevel", out Level);
        SfxSlider.value = Level;
    }

    public void SetMusicPitch(float value){
        float Level;
        Mixer.GetFloat("PitchLevel", out Level);
        var NewValue = Level * value;
        Mixer.SetFloat("PitchLevel", NewValue);
    }
}
