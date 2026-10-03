using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoGiocatore : MonoBehaviour
{
    public float velocita = 5f;
    private Rigidbody2D rb;
    private float MovimentoOrizzontale;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value) {
        Vector2 InputVector = value.Get<Vector2>();
        MovimentoOrizzontale = InputVector.x;
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(MovimentoOrizzontale * velocita, rb.linearVelocity.y);
    }
}
