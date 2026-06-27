using System.Collections;
using UnityEngine;

public class GestionLinterna : MonoBehaviour, IInteractable
{
    public ControladorLinterna linterna;
    private bool linternaEnSuelo = true; // Por defecto en el suelo (Nivel 1)

    public Transform transformMano;
    private Transform _padreOriginal;// para restaurar si la soltás

    private void Awake()
    {
        linterna = GetComponent<ControladorLinterna>();
    }
    public void EquiparLinterna()
    {
        linternaEnSuelo = false;

        // Guardás el padre original por si la soltás al suelo después
        _padreOriginal = linterna.transform.parent;

        // Emparentás a la mano
        linterna.transform.SetParent(transformMano);
        linterna.transform.localPosition = Vector3.zero;
        linterna.transform.localRotation = Quaternion.identity;

        // Desactivás físicas para que no caiga sola
        if (linterna.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        linterna.gameObject.SetActive(true);
        Debug.Log("Linterna equipada.");
    }

    public void SoltarLinterna()
    {
        linternaEnSuelo = true;

        // Desemparentás y restaurás físicas para que caiga
        linterna.transform.SetParent(_padreOriginal);

        if (linterna.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        linterna.Apagar();
        Debug.Log("Linterna soltada.");
    }

    public void SoltarLinternaPorSusto()
    {
        SoltarLinterna();

        // Impulso aleatorio al soltar por susto
        if (linterna.TryGetComponent(out Rigidbody rb))
        {
            Vector3 impulso = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(0.5f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized * 3f;
            rb.AddForce(impulso, ForceMode.Impulse);
        }

        Debug.Log("¡Dani soltó la linterna por el susto!");
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

    public void Interactuar(ControladorPersonaje jugador)
    {
        if (!linternaEnSuelo) return; // ya está equipada

        transformMano = jugador.transformMano; // tomás la mano del jugador
        EquiparLinterna();
    }
    public void IntentarAlternarLuz()
    {
        if (linternaEnSuelo) return; // no hace nada si está suelta
        linterna.AlternarLuz();
    }
}