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
    private float puloparedecooldown;
    private float horizontalinput;

    private void Awake()
    {
        //Pegar referencias do player
        corpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        horizontalinput = Input.GetAxis("Horizontal");
        corpo.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidade, corpo.velocity.y);
     
        //Mudar a direção do carinha quando estiver se mexendo para a esquerda
        if (horizontalinput > 0.01f) 
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalinput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        

        //Parametros da animation
        anim.SetBool("Correndo", horizontalinput != 0);
        anim.SetBool("No_chao", nochao());

        //Pulo da parede
        if(puloparedecooldown > 0.3f)
        {
            
            if (naparede() && !nochao())
            {
                corpo.gravityScale = 0;
                //corpo.velocity = new Vector2(transform.localScale.x, -1f);

            }
            else
            {
                corpo.gravityScale = 1.63f;
            }

            //Pular
            if (Input.GetKey(KeyCode.Space))
            {
                Pular();
            }
        }
        else
        {
            puloparedecooldown += Time.deltaTime;
        }
    }

    private void Pular()
    {
        if (nochao())
        {
            corpo.velocity = new Vector2(corpo.velocity.x, impulsodopulo);
            anim.SetTrigger("Pular");
        }
        else if(naparede() && !nochao())
        {
            if (horizontalinput == 0)
            {
                corpo.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) + 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                corpo.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) + 3, 10);
            }

            puloparedecooldown = 0f;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
}


