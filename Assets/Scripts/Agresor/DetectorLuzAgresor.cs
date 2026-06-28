using UnityEngine;

public class DetectorLuzAgresor : MonoBehaviour
{
    [SerializeField] private Agresor agresor;
    [SerializeField] private ControladorLinterna linterna;
    [SerializeField] private LayerMask capasAIgnorar; // Asigná la layer "Agresor" acá

    [Tooltip("Cada cuántos segundos verifica si la luz lo está iluminando")]
    [SerializeField] private float intervaloChequeo = 0.2f;

    private float timerChequeo = 0f;

    private void Update()
    {
        if (linterna == null || agresor == null) return;

        // Solo chequea si la linterna está encendida
        if (!linterna.EstaEncendida()) return;

        timerChequeo += Time.deltaTime;
        if (timerChequeo >= intervaloChequeo)
        {
            timerChequeo = 0f;
            VerificarSiLuzIluminaAgresor();
        }
    }

    private void VerificarSiLuzIluminaAgresor()
    {
        Light luz = linterna.componenteLuz;
        if (luz == null) { Debug.Log(" No encuentra el componenteLuz"); return; }

        Vector3 posLuz = luz.transform.position;
        Vector3 dirAgresor = (transform.position - posLuz).normalized;
        float distancia = Vector3.Distance(posLuz, transform.position);

        Debug.Log($"Distancia a la luz: {distancia} | Umbral: {agresor.umbralLuzDeteccion}");

        if (distancia > agresor.umbralLuzDeteccion)
        { Debug.Log(" Muy lejos del umbral"); return; }

        float angulo = Vector3.Angle(luz.transform.forward, dirAgresor);
        Debug.Log($"Ángulo: {angulo} | SpotAngle/2: {luz.spotAngle / 2f}");

        if (angulo > luz.spotAngle / 2f)
        { Debug.Log(" Fuera del cono de luz"); return; }

        RaycastHit hit;
        if (Physics.Raycast(posLuz, dirAgresor, out hit, distancia, ~capasAIgnorar))
        { Debug.Log($" Obstáculo en el camino: {hit.collider.gameObject.name}"); return; }

        Debug.Log("Luz detectada, llamando DetectarLuz()");
        agresor.DetectarLuz(linterna.intensidadLuz, posLuz);
    }
}