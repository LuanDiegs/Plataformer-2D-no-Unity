using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Animator anim;

    private Rigidbody2D bola;

    private BoxCollider2D boxcollider;

    [SerializeField]private float speed;
    private float direcao;

    private bool hit;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        bola = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        if (hit) 
        {
            return; 
        }

       //print(boxcollider.enabled);

        float vel_movimentacao = speed * Time.deltaTime * direcao;
        transform.Translate(vel_movimentacao, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        BoxCollider2D Jogador = GameObject.FindWithTag("Jogador").GetComponent<BoxCollider2D>();

        //print(collision);
        //print(Jogador);

        if (collision == Jogador    ) 
        {
            hit = false;
            boxcollider.enabled = true;
            return;
        }

        hit = true;
        boxcollider.enabled = false;
        anim.SetTrigger("Explodir");
    }

    public void direcaobola(float _direcao)
    {
        direcao = _direcao;
        gameObject.SetActive(true);
        hit = false;
        boxcollider.enabled = true; 

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direcao)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void desativarbola() 
    {
        gameObject.SetActive(false);
    }
}
