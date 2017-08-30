﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {//Script utilizado para controlar o jogador

    public float moveSpeed; // Velocidade de movimento do jogador
    private Rigidbody2D myRigidBody; //variavel do tipo Rigidbody2d. Será usada para acessar os atributos desse componente no objeto 
    private SpriteRenderer myRenderer;//variável do tipo Sprite Renderer. Será usada para acessar os atributos desse componente, asssim como flipar o jogador

    public float jumpSpeed;//variável que ditará a força do pulo do jogador

    public Transform groundCheck;//posição de GroundCheck
    public float groundCheckRadius;//
    public LayerMask whatIsGround;//
    public bool isGrounded;

    private Animator myAnim;// Variavel do tipo animator, que será usada para acessar seus atributos e mudar os bools para mudar animações
    public Vector3 respawnPosition; //vetor de 3 posições, referente a posição de ressurgimento do jogador

    public LevelManager levelManager;

	// Use this for initialization  
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>(); //Linka o RigidBody2D do gameObject escolhido á variável myRigidBody . 
        myAnim = GetComponent<Animator>();//Linka o Animator do gameObject escolhido á variável myAnim .
        myRenderer = GetComponent<SpriteRenderer>();//Associa o Sprite Renderer do objeto á variavel myRenderer, no comeco do jogo
        respawnPosition = transform.position;//Posição inicial do RespawnPoint será a posição incial do Player. (Caso não atinja os checkpoints).
        levelManager = FindObjectOfType<LevelManager>();//Linka o objeto Level Manager á variável levelManager, para poder manipular seus atributos
        respawnPosition = transform.position; //posição de Respawn inicial (caso não tenha atingido checkpoints) é a posição inicial do jogador no jogo. Ou seja, volta pra posição default.
    }   
	
	// Update is called once per frame
	void Update () {

        isGrounded = Physics2D.OverlapCircle //OverlapCircle é um circulo imaginário desenhado no pé do personagem
            ( groundCheck.position, //recebe a posição do object que está pregado no pé do player, para saber que o check deve ocorrer nessa área
             groundCheckRadius,//raio definido para o gameobject que servirá de sensor de toque no chao
               whatIsGround);//variável que define o que pode ser considerado como Ground ou terreno andável.
           


		if(Input.GetAxisRaw("Horizontal") > 0f) // Caso o Input na horizontal (eixo X) seja maior que 0, o personagem anda pra direita
        {
            myRigidBody.velocity = new Vector3(moveSpeed, myRigidBody.velocity.y, 0f);
            myRenderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f) // Caso o Input na horizontal (eixo X) seja menor que 0, o personagem anda pra direita
        {
            myRigidBody.velocity = new Vector3(-moveSpeed, myRigidBody.velocity.y, 0f);
            myRenderer.flipX = true; //flipa o personagem, pois o sprite está virado para a esquerda.
        }
        else
        {
            myRigidBody.velocity = new Vector3(0, myRigidBody.velocity.y, 0f); //fica parado

        }
        if (Input.GetButtonDown("Jump") && isGrounded==true)// Caso o botão associado ao pulo no Input for pressionado e o jogador estiver no solo.
        {
            myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpSpeed, 0f); //pega a velocidade no X, e junta com a força do pulo. Num Vetor de 2 variáveis.

        }
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));//Faz com que a variável Speed retorne apenas valores verdadeiros.
        myAnim.SetBool("Grounded", isGrounded);
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "KillPlane") //caso encoste no colisor referente ao Chão/abismo
        {

            levelManager.Respawn(); // Invoca o método respawn do Level Manager
            // gameObject.SetActive(false);
            //transform.position = respawnPosition; // A posição inicial do player passa a ser a posição do último checkpoint
        }
        if(collision.tag == "Checkpoint")
        {
            respawnPosition = collision.transform.position; // a posição do choque do player e o checkpoint fica salva na variável respawnPoint;
            levelManager.waitToRespawn = 2;//tempo que demora para o jogador respawnar quando for voltar para um checkpoint

        }



    }


}
