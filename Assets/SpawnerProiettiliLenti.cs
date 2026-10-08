using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using System;

public class SpawnerProiettiliLenti : MonoBehaviour
{
    #region STRUTTURE DATI

    [Serializable]
    public struct DatiSpawn
    {
        public float Cooldown;
        public float Numero;
        public float Direzione;
        public float CooldownIniziale;
    }

    #endregion
        
    #region CONFIGURAZIONI VARIABILI
        public DatiSpawn spawn = new DatiSpawn { Cooldown = 3f, Numero = 1, Direzione = -1, CooldownIniziale = 2f };
        public GameObject ProiettileLento;
    #endregion

    #region METODI BASE DI UNITY
        void Start()
        {
            StartCoroutine(RoutineSparo());
        }

        void FixedUpdate()
        {
            
        }
    #endregion
    
    #region METODI DEDICATI ALLO SPAWN
        private IEnumerator RoutineSparo()
        {   
            yield return new WaitForSeconds(spawn.CooldownIniziale);

            while (true)
            {
                Spara();
                yield return new WaitForSeconds(spawn.Cooldown);
            }
        }

        void Spara()
        {
            if(ProiettileLento == null)
            {
                Debug.LogWarning("non esistono prefab assegnati");
                return;
            }
            Instantiate(ProiettileLento, transform.position, transform.rotation);
        }
    #endregion

}