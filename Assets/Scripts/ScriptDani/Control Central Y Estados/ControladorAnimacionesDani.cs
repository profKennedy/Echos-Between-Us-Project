using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAnimacionesDani : MonoBehaviour
{
    private Animator _animator;
    private ControladorPersonaje _controlador;
    private MovimientoPersonaje _movimiento;

    // Hashes son más performantes que strings
    private static readonly int EstaCorriendo = Animator.StringToHash("EstaCorriendo");
    private static readonly int EnSigilo = Animator.StringToHash("EnSigilo");
    private static readonly int EstaSaltando = Animator.StringToHash("EstaSaltando");
    private static readonly int EstaAsustada = Animator.StringToHash("EstaAsustada");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _controlador = GetComponent<ControladorPersonaje>();
        _movimiento = GetComponent<MovimientoPersonaje>();
    }

    private void OnEnable()
    {
        _controlador.OnEstadoCambiado += AlCambiarEstado;
    }

    private void OnDisable()
    {
        _controlador.OnEstadoCambiado -= AlCambiarEstado;
    }

    private void Update()
    {
        // Correr: si la velocidad horizontal es mayor a un umbral
        float velocidadHorizontal = new Vector2(
            _movimiento.rb.velocity.x,
            _movimiento.rb.velocity.z
        ).magnitude;

        _animator.SetInteger(EstaCorriendo, velocidadHorizontal > 0.1f ? 1 : 0);

        // Saltar: si no está en el suelo
        _animator.SetBool(EstaSaltando, !_movimiento.EstaEnSuelo());
    }

    private void AlCambiarEstado(EstadoPersonaje nuevoEstado)
    {
        // Resetear todo primero
        _animator.SetBool(EnSigilo, false);
        _animator.SetBool(EstaAsustada, false);

        switch (nuevoEstado)
        {
            case EstadoPersonaje.SIGILO:
                _animator.SetBool(EnSigilo, true);
                break;

            case EstadoPersonaje.ASUSTADA:
                _animator.SetBool(EstaAsustada, true);
                break;

            case EstadoPersonaje.LIBRE:
            case EstadoPersonaje.EN_FLASHBACK:
                // Libre y Flashback no tienen parámetro propio,
                // el bloqueo de movimiento en FLASHBACK lo maneja MovimientoPersonaje
                break;
        }
    }
}
