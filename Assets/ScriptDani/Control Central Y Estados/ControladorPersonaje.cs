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
    public GestionLinterna gestionLinterna;

    public event Action OnPersonajeAsustado;
    public event Action<EstadoPersonaje> OnEstadoCambiado;
    [Space]
    public Camera camaraFreeLook;  // arrastrás la cámara Cinemachine Brain acá
    public Transform transformMano;

    private ModoFreeLook _modoFreeLook;
    private Modo2DLateral _modo2D;

    private void Awake()
    {
        _modoFreeLook = new ModoFreeLook(camaraFreeLook.transform);
        _modo2D = new Modo2DLateral();
    }
    private void Start()
    {
        _modoFreeLook = new ModoFreeLook(camaraFreeLook.transform);
        _modo2D = new Modo2DLateral();

        // Arranca en FreeLook por defecto
        movimiento.modoActual = _modoFreeLook;

        RunSelfCheck();
    }

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
            entradas.AlInteractuar += interaccion.IntentarInteractuar;
            entradas.AlAlternarLinterna += gestionLinterna.linterna.AlternarLuz; // F para encender/apagar
        }
        // Nota: Deberás conectar interaccion.IntentarInteractuar a entradas.AlInteractuar también
    }
    private void OnDisable()
    {
        if (entradas != null && movimiento != null)
        {
            entradas.AlMoverse -= movimiento.Mover;
            entradas.AlSaltar -= movimiento.Saltar;
            entradas.AlInteractuar -= interaccion.IntentarInteractuar;
            entradas.AlAlternarLinterna -= gestionLinterna.linterna.AlternarLuz;
        }
    }

    public void UsarCamaraFreeLook() => movimiento.modoActual = _modoFreeLook;
    public void UsarCamara2D() => movimiento.modoActual = _modo2D;
}
