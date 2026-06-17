using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoEncogimiento : MonoBehaviour
{
    public float factorEncogimientoActual = 1f;
    public float factorMinimo = 0.2f;
    public Transform transformPersonaje;
    public float velocidadTransicion = 5f;

    public void AplicarEncogimiento(float nivelNegatividadZona)
    {
        factorEncogimientoActual = Mathf.Max(factorMinimo, 1f - nivelNegatividadZona);
        transformPersonaje.localScale = Vector3.one * factorEncogimientoActual; // ponytail: escala directa en vez de Lerp por frame.
    }

    public void RevertirEncogimiento() => transformPersonaje.localScale = Vector3.one;
    public float ObtenerFactorActual() => factorEncogimientoActual;
    public bool EstaEncogida() => factorEncogimientoActual < 0.98f;
}
