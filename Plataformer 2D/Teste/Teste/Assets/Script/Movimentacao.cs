using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    [SerializeField] private LayerMask Chaolayer;
    [SerializeField] private LayerMask Paredelayer;

    private Rigidbody2D corpo;

    private Animator anim;

    private BoxCollider2D boxcollider;
   
    [SerializeField] private float velocidade;

    [SerializeField] private float impulsodopulo;

    //Mexer horizontalmente
    private float inputhor;

    //Pulo da parede
    [SerializeField] private float puloparede_x;
    [SerializeField] private float puloparede_y;
    private bool pulandoparede;
    [SerializeField] private float puloparedetempo;

    //Cooldown do pulo na parede
    float cooldowntempo = 0.5f;
    bool emcooldown = false;

    //Deslizar na parede
    [SerializeField] private float velocidadedeslizar;
    private bool deslizando;

    private void Awake()
    {
        //Pegar referencias do player
        corpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        inputhor = Input.GetAxisRaw("Horizontal");
        corpo.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidade, corpo.velocity.y);
     
        //Mudar a direção do carinha quando estiver se mexendo para a esquerda
        if (inputhor > 0.01f) 
        {
            transform.localScale = Vector3.one;
        }
        else if (inputhor < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        //Parametros da animatior
        anim.SetBool("Correndo", inputhor != 0);
        anim.SetBool("No_chao", nochao());

        //Pulo da parede          
        if (naparede() && !nochao())
        {
            //corpo.velocity = new Vector2(transform.localScale.x, -1f);

            if (inputhor == 0)
            {
                deslizando = false;
                corpo.velocity = new Vector2(transform.localScale.x, -velocidadedeslizar);
            }
            else
            {
                deslizando = true;
                corpo.velocity = new Vector2(transform.localScale.x, -velocidadedeslizar);
            }        
        }
        else
        {
            deslizando = false;
            corpo.gravityScale = 1.63f;
        }

        //Pular
        if (Input.GetKey(KeyCode.Space))
        {
            Pular();
        }

        //Cooldown
        if (emcooldown)
        {
            cooldowntempo -= Time.deltaTime;
            if (cooldowntempo <= 0)
            {
                emcooldown = false;
                cooldowntempo = 0.5f;
            }
        }

        //Teste
        print(cooldowntempo);
    }

    private void Pular()
    {
        if (nochao())
        {
            corpo.velocity = new Vector2(corpo.velocity.x, impulsodopulo);
            anim.SetTrigger("Pular");
        }
        else if (deslizando)
        {
            pulandoparede = true;
            Invoke("pulandoparedefalso", puloparedetempo);
        }

        if (!emcooldown)
        {
            if (pulandoparede)
            {
                corpo.velocity = new Vector2(puloparede_x * -inputhor, puloparede_y);
            }
        }
    }         

    private bool nochao() 
    {
        RaycastHit2D tocando = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, Chaolayer);
        return tocando.collider != null;
    }
    private bool naparede()
    {
        RaycastHit2D tocando = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, Paredelayer);
        return tocando.collider != null;
    }

    private void pulandoparedefalso() 
    {
        pulandoparede = false;
        emcooldown = true;
    }
}