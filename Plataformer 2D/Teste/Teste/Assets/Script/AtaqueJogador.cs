using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    [SerializeField] private float ataquecooldown;
    private float cooldowntimerataque = Mathf.Infinity;

    private Animator anim;

    private Movimentacao movi;

    [SerializeField] private Transform pontodeiniciodotiro;
    [SerializeField] private GameObject[] bolas_de_fogo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movi = GetComponent<Movimentacao>();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldowntimerataque > ataquecooldown && movi.podeatacar())
        {
            Atacar();
        }

        cooldowntimerataque += Time.deltaTime;

        //print(cooldowntimerataque);
    }

    private void Atacar()
    {
        anim.SetTrigger("Atacar");
        cooldowntimerataque = 0;

        bolas_de_fogo[nro_bolas_de_fogo()].transform.position = pontodeiniciodotiro.position;
        bolas_de_fogo[nro_bolas_de_fogo()].GetComponent<Projetil>().direcaobola(Mathf.Sign(transform.localScale.x));
    }

    private int nro_bolas_de_fogo()
    {
        for (int i = 0; i < bolas_de_fogo.Length; i++)
        {
            if (!bolas_de_fogo[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
