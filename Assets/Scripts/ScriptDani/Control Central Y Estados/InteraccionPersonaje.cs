using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionPersonaje : MonoBehaviour
{
    public float radioDeteccion = 2f;
    public LayerMask layerInteractuable;

    public void IntentarInteractuar()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radioDeteccion, layerInteractuable);
        if (hits.Length > 0 && hits[0].TryGetComponent(out IInteractable target)) target.Interactuar(GetComponent<ControladorPersonaje>());
    }

    public void PerderLinternaPorSusto() { } // ponytail: lógica omitida hasta linkear módulo Linterna.
    public bool EsLinternaCercana() => false;
}
