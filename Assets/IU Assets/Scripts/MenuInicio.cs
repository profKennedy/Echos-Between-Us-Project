using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public void EmpezarJuego()
    {
        SceneManager.LoadScene("Linterna y Sombras");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}