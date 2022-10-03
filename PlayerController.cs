using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public AudioSource Aud;
        public AudioClip stepClip, jumpClip, enemyClip, coinClip;
        bool isPlaying = false;

        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        void Start()
        {
            Aud.clip = stepClip;
            Aud.clip = jumpClip;
            Aud.clip = enemyClip;
            Aud.clip  = coinClip;

            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetButton("Horizontal")) 
            {
                //Sonido movimiento 
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }
            if(Input.GetButtonDown("Horizontal")){
                isPlaying = true;
                ReproducirStep();
            }
            else if(Input.GetButtonUp("Horizontal")){
                isPlaying = false;   
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                //Sonido saltar por AddForce
                isPlaying = true;
                ReproducirJump();
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            else if(Input.GetKeyUp(KeyCode.Space)){
                isPlaying = false;

            }
            if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation

            if(facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if(facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                //Muerte cuando choque o toque un enemigo o una barrera
                ReproducirEnemy();
                deathState = true; //Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Cuando coja una moneda
            if (other.gameObject.tag == "Coin")
            {
                isPlaying = true;
                ReproducirCoin();
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
                //Cuando coja una moneda
            }
        }
        public void ReproducirStep(){
            Aud.clip = stepClip;
            StartCoroutine(play(1.0f)); 
        }
        public void DetenerStep(){
            Aud.clip = stepClip;
            Aud.Stop();
        }
        IEnumerator play(float tiempo){
            while(isPlaying){
                Aud.Play();
                yield return new WaitForSeconds(tiempo);
            }
        }
        public void ReproducirJump(){
            Aud.clip = jumpClip;
            StartCoroutine(play(1.0f)); 
        }
        public void DetenerJump(){
            Aud.clip = jumpClip;
            Aud.Stop();
        }
        public void ReproducirEnemy(){
            Aud.clip = enemyClip;
            Aud.Play();
        }
        public void ReproducirCoin(){
            Aud.clip = coinClip;
            StartCoroutine(play(1.0f)); 
        }
    }
}
