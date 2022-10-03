using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        

        public TextMeshProUGUI timerText;

        float Timer = 59f, StartTime = 0.0f;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            StartTime = Time.time;
        }

        bool estado = true;

        void Update()
        {
            float elapsedtime = Time.time - StartTime;
            int Seconds = 0;
            if (elapsedtime < 60f) Seconds = (int)((Timer - elapsedtime) % 60);
            //Faltando 20 segundos se acelera la música  

            timerText.SetText(Seconds.ToString());

            if(Seconds <= 20){
                if (estado) GetComponent<SoundManager>().SetMusicPitch(1.1f);
                estado = false;
            }

            coinText.text = coinsCounter.ToString();
            if(player.deathState == true)
            {
                playerGameObject.SetActive(false);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                Invoke("ReloadLevel", 3);
                player.GetComponent<AudioSource>().Stop();
                GetComponent<SoundManager>().SetMusicPitch(0.8f);
            }
        }

        private void ReloadLevel()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
