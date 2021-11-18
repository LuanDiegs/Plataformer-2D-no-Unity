using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    [SerializeField] private float velocidade;
    [SerializeField] private LayerMask Chaolayer;
    private Rigidbody2D corpo;
    private Animator anim;
    private BoxCollider2D boxcollider;

    private void Awake()
    {
        //Pegar referencias do player
        corpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        corpo.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidade, corpo.velocity.y);
     
        //Mudar a direção do carinha quando estiver se mexendo para a esquerda
        if (hor > 0.01f) 
        {
            transform.localScale = Vector3.one;
        }
        else if (hor < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Pular
        if (Input.GetKey(KeyCode.Space) && nochao())
        {
            Pular();
        }

        //Parametros da animation
        anim.SetBool("Correndo", hor != 0);
        anim.SetBool("No_chao", nochao());
    }

    private void Pular()
    {
        corpo.velocity = new Vector2(corpo.velocity.x, velocidade);
        anim.SetTrigger("Pular");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool nochao() 
    {
        RaycastHit2D tocando = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, Chaolayer);
        return tocando.collider != null;
    }
}


