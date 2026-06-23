using System.Collections;
using UnityEngine;
using System;

public class ControladorLinterna : MonoBehaviour
{
    [Header("Atributos de Batería")]
    public float bateriaMaxima = 100f;
    public float bateriaActual = 100f;
    public float velocidadConsumoBateria = 5f;

    [Header("Atributos de Luz")]
    public float intensidadLuz = 10f;
    private bool estaEncendida = false;

    // Eventos 
    public Action<bool> OnLuzCambiada;
    public Action<float> OnBateriaCambiada;

    // Uso de Referencia al componente Light de Unity
    private Light componenteLuz;

    private void Awake()
    {
        componenteLuz = GetComponent<Light>();
        bateriaActual = bateriaMaxima;
    }

    private void Update()
    {
        if (estaEncendida)
        {
            ConsumirBateriaPorFrame();
        }
    }

    public void Encender()
    {
        if (bateriaActual > 0)
        {
            estaEncendida = true;
            if (componenteLuz != null) componenteLuz.enabled = true;
            OnLuzCambiada?.Invoke(true);
        }
    }

    public void Apagar()
    {
        estaEncendida = false;
        if (componenteLuz != null) componenteLuz.enabled = false;
        OnLuzCambiada?.Invoke(false);
    }

    public void AlternarLuz()
    {
        if (estaEncendida) Apagar();
        else Encender();
    }

    public void ConsumirBateriaPorFrame()
    {
        bateriaActual -= velocidadConsumoBateria * Time.deltaTime;

        if (bateriaActual <= 0)
        {
            bateriaActual = 0;
            Apagar();
        }
        OnBateriaCambiada?.Invoke(ObtenerPorcentajeBateria());
    }

    public void RecargarBateria(float cantidad)
    {
        bateriaActual += cantidad;
        if (bateriaActual > bateriaMaxima) bateriaActual = bateriaMaxima;
        OnBateriaCambiada?.Invoke(ObtenerPorcentajeBateria());
    }

    public float ObtenerPorcentajeBateria()
    {
        return bateriaActual / bateriaMaxima;
    }

    public bool EstaEncendida()
    {
        return estaEncendida;
    }
}