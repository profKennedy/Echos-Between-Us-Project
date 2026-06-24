using System.Collections;
using UnityEngine;

public class GestionLinterna : MonoBehaviour
{
    public ControladorLinterna linterna;
    private bool linternaEnSuelo = true; // Por defecto en el suelo (Nivel 1)

    public void EquiparLinterna()
    {
        linternaEnSuelo = false;
        // esto para Emparentar el GameObject de la linterna a la mano de Dani
        linterna.gameObject.SetActive(true);
        Debug.Log("Linterna equipada.");
    }

    public void SoltarLinterna()
    {
        linternaEnSuelo = true;
        // Desemparentar y activar físicas (Rigidbody) para que caiga
        linterna.Apagar();
        Debug.Log("Linterna soltada.");
    }

    public void SoltarLinternaPorSusto()
    {
        // esto es Similar a SoltarLinterna, pero quizás con un impulso de fuerza aleatorio hay que probar
        SoltarLinterna();
        Debug.Log("¡Dany soltó la linterna por el susto!");
    }

    public bool LinternaDisponibleEnSuelo()
    {
        return linternaEnSuelo;
    }

    public void RecogerDeSuelo()
    {
        if (LinternaDisponibleEnSuelo())
        {
            EquiparLinterna();
        }
    }
}