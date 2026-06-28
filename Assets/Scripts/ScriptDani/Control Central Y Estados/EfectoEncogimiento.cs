using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoEncogimiento : MonoBehaviour
{
    public float factorEncogimientoActual = 1f;
    public float factorObjetivo = 1f;
    public float factorMinimo = 0.2f;
    public Transform transformMesh;             // solo el mesh, NO el root con collider
    public CapsuleCollider capsule;             // para ajustar altura físicamente
    public float velocidadTransicion = 2f;
    public float velocidadRecuperacion = 1f;


    private float _alturaOriginal;
    private float _radioOriginal;

    private void Awake()
    {
        if (capsule != null)
        {
            _alturaOriginal = capsule.height;
            _radioOriginal = capsule.radius;
        }
    }
    private void Update()
    {
        // Interpolar suavemente hacia el objetivo
        float velocidad = factorEncogimientoActual > factorObjetivo
            ? velocidadTransicion
            : velocidadRecuperacion;

        factorEncogimientoActual = Mathf.MoveTowards(
            factorEncogimientoActual,
            factorObjetivo,
            velocidad * Time.deltaTime
        );

        // Solo escalar el mesh visual
        if (transformMesh != null)
            transformMesh.localScale = Vector3.one * factorEncogimientoActual;

        // Ajustar el collider proporcionalmente
        if (capsule != null)
        {
            capsule.height = _alturaOriginal * factorEncogimientoActual;
            capsule.radius = _radioOriginal * factorEncogimientoActual;
        }
    }
    // Llamado por ReceptorMiradaFantasma cada frame que la miran
    public void AplicarPresion(float nivelNegatividad)
    {
        factorObjetivo = Mathf.Max(factorMinimo, 1f - nivelNegatividad);
    }

    // Llamado cuando dejan de mirarla
    public void IniciarRecuperacion()
    {
        factorObjetivo = 1f;
    }
    public float ObtenerFactorActual() => factorEncogimientoActual;
    public bool EstaEncogida() => factorEncogimientoActual < 0.98f;
}
