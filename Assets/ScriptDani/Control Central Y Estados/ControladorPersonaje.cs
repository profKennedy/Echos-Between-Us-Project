using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{
    public EstadoPersonaje estado;
    public MovimientoPersonaje movimiento;
    public InteraccionPersonaje interaccion;
    public EfectoEncogimiento efectoEncogimiento;

    public GestorEntradas entradas;


    public event Action OnPersonajeAsustado;
    public event Action<EstadoPersonaje> OnEstadoCambiado;

    private void Start() => RunSelfCheck();

    public void CambiarEstado(EstadoPersonaje nuevoEstado)
    {
        if (estado == nuevoEstado) return;
        estado = nuevoEstado;
        OnEstadoCambiado?.Invoke(estado);
    }

    public void AlSerAsustada()
    {
        CambiarEstado(EstadoPersonaje.ASUSTADA);
        OnPersonajeAsustado?.Invoke();
    }

    private void RunSelfCheck()
    {
        CambiarEstado(EstadoPersonaje.SIGILO);
        Debug.Assert(estado == EstadoPersonaje.SIGILO, "ponytail: Falló el cambio de estado básico en init.");
    }

    private void OnEnable()
    {
        if (entradas != null && movimiento != null)
        {
            entradas.AlMoverse += movimiento.Mover;
            entradas.AlSaltar += movimiento.Saltar;
        }
        // Nota: Deberás conectar interaccion.IntentarInteractuar a entradas.AlInteractuar también
    }
    private void OnDisable()
    {
        if (entradas != null && movimiento != null)
        {
            entradas.AlMoverse -= movimiento.Mover;
            entradas.AlSaltar -= movimiento.Saltar;
        }
    }
}
