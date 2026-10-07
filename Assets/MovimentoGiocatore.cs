using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovimentoGiocatore : MonoBehaviour
{
    public float velocita;
    public float velocitaBase = 5f;
    public float velocitaCorsa = 9f;
    public float salto = 7f;
    private Rigidbody2D rb;
    private float MovimentoOrizzontale;
    private float MovimentoVerticale;
    private bool jump;
    private bool SaltoPossibile = true;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        velocita = velocitaBase;
    }

    void OnMove(InputValue value) 
    {
        Vector2 InputVector = value.Get<Vector2>();
        MovimentoOrizzontale = InputVector.x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && SaltoPossibile)
        {
            jump = true;
            SaltoPossibile = false;
            //DisabilitaSaltoPerTempo(2*(salto/9.81f));
        }
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            velocita = velocitaCorsa;
        }
        else
        {
            velocita = velocitaBase;
        }
    }

    void FixedUpdate() 
    {
        MovimentoVerticale = rb.linearVelocity.y;

        if (jump)
        {
            MovimentoVerticale = salto;
            jump = false;
        }

        rb.linearVelocity = new Vector2(MovimentoOrizzontale * velocita, MovimentoVerticale);
    }


    // Metodo pubblico per avviare il blocco del salto
    public void DisabilitaSaltoPerTempo(float secondi) {
        StartCoroutine(RoutineDisabilitaSalto(secondi));
    }

    // Coroutine che gestisce l'attesa temporizzata
    private IEnumerator RoutineDisabilitaSalto(float secondi) {
        SaltoPossibile = false;
        yield return new WaitForSeconds(secondi); 
        SaltoPossibile = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pavimento"))
        {
            SaltoPossibile = true;
        }     
    }
}
