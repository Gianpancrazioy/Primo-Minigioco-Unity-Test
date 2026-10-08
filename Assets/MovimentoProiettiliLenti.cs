using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using System;

public class MovimentoProiettiliLenti : MonoBehaviour
{
    #region STRUTTURE DATI

    [Serializable]
    public struct DatiMovimento 
    {
        public float Velocita;
    } 

    [Serializable]
    public struct DatiDanno
    {
        public float Danno;
    }

    #endregion
        
    #region CONFIGURAZIONI VARIABILI
        DatiMovimento movimento = new DatiMovimento { Velocita = 3f };
        DatiDanno danno = new DatiDanno { Danno = 1f };

        private float MovimentoOrizzontale;
        private float MovimentoVerticale;
        private Rigidbody2D rb;
        private MovimentoGiocatore giocatore;
    #endregion

    #region METODI BASE DI UNITY
        void Start()
        {
            giocatore = GameObject.Find("Giocatore").GetComponent<MovimentoGiocatore>();
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate() 
        {
            rb.linearVelocity = new Vector2(MovimentoOrizzontale * movimento.Velocita, MovimentoVerticale * movimento.Velocita);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                MovimentoGiocatore target = collision.gameObject.GetComponent<MovimentoGiocatore>();

                if(target != null)
                    {
                        target.PrendiDanno(danno.Danno);
                    }
                    else {
                        Debug.LogWarning("Tag Giocatore trovato, ma manca il componente MovimentoGiocatore!");
                    }

                    Destroy(gameObject);
                }
            }
    #endregion
}