using UnityEngine;
using System;

public class SpuntoneTerreno : MonoBehaviour
{
    #region STRUTTURE DATI
    [Serializable]
    public struct DatiDanno 
    {
        public int Danno;
    } 

    [Serializable]
    public struct DatiTempo
    {
        public float Tempo;
    }
    #endregion

    #region CONFIGURAZIONE VARIABILI
    [SerializeField] DatiDanno danno = new DatiDanno { Danno = 2 };
    [SerializeField] DatiTempo tempo = new DatiTempo { Tempo = 2f };

    private MovimentoGiocatore giocatore;
    private Rigidbody2D rb;
    #endregion

    #region FUNZIONI BASE DI UNITY E COLLISIONI
        public void Start()
        {
            Destroy(gameObject, tempo.Tempo);
        }

        public void OnTriggerEnter2D(Collider2D Collision)
        {
            if (Collision.gameObject.CompareTag("Player"))
            {
                MovimentoGiocatore giocatore = Collision.gameObject.GetComponent<MovimentoGiocatore>();
                giocatore.PrendiDanno(danno.Danno);
            }
        }
    #endregion


}