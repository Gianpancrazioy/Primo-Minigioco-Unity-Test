using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using System;

public class MovimentoGiocatore : MonoBehaviour
{
    [Serializable]
    public struct DatiMovimento
    {
        [HideInInspector] public float Velocita;
        public float VelocitaBase;
        public float VelocitaCorsa;
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

    public DatiMovimento movimento = new DatiMovimento { VelocitaBase = 5f, VelocitaCorsa = 11f };
    public DatiSalto salto = new DatiSalto { SaltoPossibile = true, ForzaSalto = 7f };
    public DatiDash dash = new DatiDash { DashPossibile = true, InDash = false, DurataDash = 0.2f, CooldownDash = 0.5f, ForzaDash = 18f };
    
    private float MovimentoOrizzontale;
    private float MovimentoVerticale;
    private Rigidbody2D rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        movimento.Velocita = movimento.VelocitaBase;
    }

    void OnMove(InputValue value) 
    {
        Vector2 InputVector = value.Get<Vector2>();
        MovimentoOrizzontale = InputVector.x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && salto.SaltoPossibile)
        {
            salto.InSalto = true;
            salto.SaltoPossibile = false;
            //DisabilitaSaltoPerTempo(2*(salto/9.81f));
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


    void OnDash(InputValue value)
    {
        if (dash.DashPossibile && value.isPressed)
        {
            StartCoroutine(EseguiDash());
        }
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

    // Coroutine che gestisce l'attesa temporizzata
    private IEnumerator RoutineDisabilitaSalto(float secondi) {
        salto.SaltoPossibile = false;
        yield return new WaitForSeconds(secondi); 
        salto.SaltoPossibile = true;
    }

    private IEnumerator EseguiDash() {
        dash.DashPossibile = false;
        dash.InDash = true;

        float gravita = rb.gravityScale;
        rb.gravityScale = 0f;

        float direzione = MovimentoOrizzontale != 0 ? Mathf.Sign(MovimentoOrizzontale) : transform.localScale.x;
        rb.linearVelocity = new Vector2(dash.ForzaDash * direzione, 0f);

        yield return new WaitForSeconds(dash.DurataDash);
       
        rb.gravityScale = gravita;
        movimento.Velocita = movimento.VelocitaBase;

        yield return new WaitForSeconds(dash.CooldownDash); 
        
        dash.InDash = false;
        dash.DashPossibile = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pavimento"))
        {
            salto.SaltoPossibile = true;
        }     
    }
}
