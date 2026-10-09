using UnityEngine;
using System.Collections;
using System;

public class SpawnerManager : MonoBehaviour
{
    #region STRUTTURE DATI
    [Serializable]
    public struct DatiSpawn
    {
        public int NumMax;
        public float CooldownSpawn;
        public float CooldownInterWave;
    }
    #endregion

    #region CONFIGURAZIONE VARIABILI
    public DatiSpawn spawn = new DatiSpawn { NumMax = 2, CooldownInterWave = 15f, CooldownSpawn = 2f };

    public GameObject Spawner;

    [Header("Limiti Area di Spawn (X e Y)")]
    [SerializeField] private float xMinima = -23f;
    [SerializeField] private float xMassima = 22f;
    [SerializeField] private float yMinima = 15f;
    [SerializeField] private float yMassima = 20f;
    #endregion

    #region METODI BASE DI UNITY E COLLISIONI
    public void Start()
    {
        StartCoroutine(RoutineSpawner());
    }

    public void FixedUpdate()
    {
        
    }
    #endregion

    #region METODI PER LO SPAWN DEGLI SPAWNER
    public IEnumerator RoutineSpawner()
    {
        while(true){
            for(int i = 0; i < spawn.NumMax; i++)
            {
                float RandomY = UnityEngine.Random.Range(yMassima, yMinima);
                float RandomX = UnityEngine.Random.Range(xMassima, xMinima);

                Vector2 posizione = new Vector2(RandomX, RandomY);

                Spawn(posizione);
                yield return new WaitForSeconds(spawn.CooldownSpawn);    
            }
            
            yield return new WaitForSeconds(spawn.CooldownInterWave);
        }
    }

    public void Spawn(Vector2 posizione)
    {
        if(Spawner == null)
        {
            Debug.LogWarning("Errore, nessuno spawner assegnato");
        }

        Instantiate(Spawner, posizione, Quaternion.identity);
    }
    #endregion

}