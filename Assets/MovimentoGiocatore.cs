using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using System;

public class MovimentoGiocatore : MonoBehaviour
{
    #region STRUTTURE DATI
        [Serializable]
        public struct DatiMovimento
        {
            [HideInInspector] public float Velocita;
            public float VelocitaBase;
            public float VelocitaCorsa;
            public float UltimaDirezione;
        }

        [Serializable]
        public struct DatiSalto
        {
            public float ForzaSalto;
            public bool InSalto;
            public bool SaltoPossibile;
        }

        [Serializable]
        public struct DatiDash
        {
            public bool DashPossibile;
            public bool InDash;
            public float DurataDash;
            public float CooldownDash;
            public float ForzaDash;
        }
    #endregion


    #region CONFIGURAZIONI GIOCATORE
        public DatiMovimento movimento = new DatiMovimento { VelocitaBase = 5f, VelocitaCorsa = 11f, UltimaDirezione = 1f };
        public DatiSalto salto = new DatiSalto { SaltoPossibile = true, ForzaSalto = 7f };
        public DatiDash dash = new DatiDash { DashPossibile = true, InDash = false, DurataDash = 0.2f, CooldownDash = 0.5f, ForzaDash = 18f };
        
        private float MovimentoOrizzontale;
        private float MovimentoVerticale;
        private Rigidbody2D rb;
    #endregion

    #region METODI BASE DI UNITY E COLLISIONI
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        movimento.Velocita = movimento.VelocitaBase;
    }

    void FixedUpdate() 
    {
        if (dash.InDash) return;

        MovimentoVerticale = rb.linearVelocity.y;

        if (salto.InSalto)
        {
            MovimentoVerticale = salto.ForzaSalto;
            salto.InSalto = false;
        }

        rb.linearVelocity = new Vector2(MovimentoOrizzontale * movimento.Velocita, MovimentoVerticale);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pavimento"))
        {
            salto.SaltoPossibile = true;
        }     
    }
    #endregion

    #region METODI PER MOVIMENTO
    void OnMove(InputValue value) 
    {
        Vector2 InputVector = value.Get<Vector2>();
        MovimentoOrizzontale = InputVector.x;

        if (MovimentoOrizzontale != 0)
        {
            movimento.UltimaDirezione = Mathf.Sign(MovimentoOrizzontale);
        }
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            movimento.Velocita = movimento.VelocitaCorsa;
        }
        else
        {
            movimento.Velocita = movimento.VelocitaBase;
        }
    }
    #endregion

    #region METODI PER SALTO
    void OnJump(InputValue value)
    {
        if (value.isPressed && salto.SaltoPossibile)
        {
            salto.InSalto = true;
            salto.SaltoPossibile = false;
            //DisabilitaSaltoPerTempo(2*(salto/9.81f));
        }
    }
    /*
    // Coroutine che gestisce l'attesa temporizzata
    private IEnumerator RoutineDisabilitaSalto(float secondi) {
        salto.SaltoPossibile = false;
        yield return new WaitForSeconds(secondi); 
        salto.SaltoPossibile = true;
    }
    */
    #endregion

    #region METODI PER DASH
    void OnDash(InputValue value)
    {
        if (dash.DashPossibile && value.isPressed)
        {
            StartCoroutine(EseguiDash());
        }
    }

    private IEnumerator EseguiDash() {
        dash.DashPossibile = false;
        dash.InDash = true;

        float gravita = rb.gravityScale;
        rb.gravityScale = 0f;

        float direzione = MovimentoOrizzontale != 0 ? Mathf.Sign(MovimentoOrizzontale) : movimento.UltimaDirezione;
        rb.linearVelocity = new Vector2(dash.ForzaDash * direzione, 0f);

        yield return new WaitForSeconds(dash.DurataDash);
       
        rb.gravityScale = gravita;
        movimento.Velocita = movimento.VelocitaBase;

        yield return new WaitForSeconds(dash.CooldownDash); 
        
        dash.InDash = false;
        dash.DashPossibile = true;
    }
    #endregion

}
