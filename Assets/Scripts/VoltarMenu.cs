
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarMenu : MonoBehaviour
{
    public void Voltar()
    {
         //PARA FIM DE TESTE RESETA O PROGRESSO QUANDO VOLTA AO MENU
        PlayerPrefs.DeleteKey("PadariaConcluida");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Menu");
    }
}
