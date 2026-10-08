using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using System;

public class MovimentoProiettiliLenti : MonoBehaviour
{
    #region STRUTTURE DATI
    [Serializable]
    public struct DatiTempo
    {
        public float Tempo;
    }

    [Serializable]
    public struct DatiMovimento 
    {
        public float Velocita;
    } 

    [Serializable]
    public struct DatiDanno
    {
        public float DannoSpine;
        public float DannoProiettili;
    }

    [Serializable]
    public struct DatiSpine
    {
        public int Numero;
        public float Offset;
        public float CooldownIniziale;
        public float Cooldown;
    }

    #endregion
        
    #region CONFIGURAZIONI VARIABILI
        DatiMovimento movimento = new DatiMovimento { Velocita = 3f };
        DatiTempo tempo = new DatiTempo { Tempo = 30f };
        DatiDanno danno = new DatiDanno { DannoSpine = 2f, DannoProiettili = 1f };
        DatiSpine spine = new DatiSpine { Numero = 2, Offset = 1.5f, CooldownIniziale = 1.5f, Cooldown = 1f };

        private bool Hit = false;
        private Rigidbody2D rb;
        private Vector2 Direzione;
        public GameObject SpuntoneTerreno;

        [Header("Rilevamento Terreno")]
        [SerializeField] private float DistanzaRilevamentoTerreno = 1.5f; 
        [SerializeField] private LayerMask LayerPavimento;
        private SpriteRenderer spriteRenderer;
    #endregion

    #region METODI BASE DI UNITY E COLLISIONI
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            Destroy(gameObject, tempo.Tempo);
        }

        void FixedUpdate() 
        {
            if (Hit)
            {
                return;
            }

            GameObject objGiocatore = GameObject.Find("Giocatore");

            if(objGiocatore != null)
            {
                Vector2 direzione = objGiocatore.transform.position - transform.position;
                Direzione = direzione.normalized;
            }
            else
            {
                Direzione = Vector2.left;
            }
            rb.linearVelocity = Direzione * movimento.Velocita;
            
            // 2. Controllo vicinanza al terreno tramite Raycast2D verso il basso
            ControllaVicinanzaTerreno();
    }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (Hit) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                MovimentoGiocatore target = collision.gameObject.GetComponent<MovimentoGiocatore>();

                if(target != null)
                    {
                        target.PrendiDanno(danno.DannoProiettili);
                    }
                else {
                    Debug.LogWarning("Tag Giocatore trovato, ma manca il componente MovimentoGiocatore!");
                    }

                Destroy(gameObject);
            }
            
        }
    #endregion

    #region METODI PER CONTROLLO IMPATTO
        private void ControllaVicinanzaTerreno()
        {
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D HitPavimento = Physics2D.Raycast(transform.position, Vector2.down, DistanzaRilevamentoTerreno, LayerPavimento);

            if (HitPavimento.collider != null)
            {
                Hit = true;

                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;

                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }

                StartCoroutine(RoutineSpine(HitPavimento.point));
            }
        }
    #endregion

    #region METODI PER SPAWN SPINE
        private IEnumerator RoutineSpine(Vector2 Collisione)
        {   
            yield return new WaitForSeconds(spine.CooldownIniziale);

            Vector2 PosSx = Collisione + Vector2.left * spine.Offset;
            Spine(PosSx);

            Vector2 PosDx = Collisione + Vector2.right * spine.Offset;
            Spine(PosDx);

            Destroy(gameObject);                
        }

        void Spine(Vector2 Collisione)
        {
            if(SpuntoneTerreno == null)
            {
                Debug.LogWarning("non esistono prefab assegnati");
                return;
            }
            Instantiate(SpuntoneTerreno, Collisione, Quaternion.identity);
        }
    #endregion
}