using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public Rigidbody rb;
    public float velocidadBase = 5f;
    public float velocidadActual = 5f;
    public float velocidadRotacion = 10f;
    public float fuerzaSaltoBase = 6f;
    public float fuerzaSaltoActual = 6f;
    public bool enSigilo;
    public LayerMask layerSuelo;
    public IModoMovimiento modoActual;


    private bool _bloqueado = false;
    private void Awake()
    {
        modoActual = new ModoFreeLook(Camera.main.transform);
    }
    public void Mover(Vector2 dir)
    {
        if (modoActual == null|| _bloqueado) return;
        Vector3 direccion = modoActual.CalcularDireccion(dir);
        rb.velocity = new Vector3(
            direccion.x * velocidadActual,
            rb.velocity.y,
            direccion.z * velocidadActual
        );

        if (direccion.sqrMagnitude > 0.01f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                Time.deltaTime * velocidadRotacion
            );
        }
    }
    public void Bloquear(bool valor) => _bloqueado = valor;


    public void Saltar() { if (EstaEnSuelo()) rb.AddForce(Vector3.up * fuerzaSaltoActual, ForceMode.Impulse); }

    public void ActivarSigilo(bool activo)
    {
        enSigilo = activo;
        velocidadActual = activo ? velocidadBase * 0.5f : velocidadBase; // ponytail: factor de sigilo fijo.
    }

    public void AplicarModificadoresEncogimiento(float factor) => velocidadActual = velocidadBase * factor;

    public bool EstaEnSuelo()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        Vector3 origen = capsule.bounds.center;
        float radio = capsule.radius * 0.9f;
        float distancia = capsule.bounds.extents.y + 0.1f;

        return Physics.SphereCast(origen, radio, Vector3.down, out _, distancia, layerSuelo);
    }
}
