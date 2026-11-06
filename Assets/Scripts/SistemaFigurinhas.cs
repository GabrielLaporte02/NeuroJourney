
using UnityEngine;

public static class SistemaFigurinhas
{
    public static void Desbloquear(string personagem)
    {
        PlayerPrefs.SetInt("Figurinha_" + personagem, 1);
        PlayerPrefs.Save();
    }

    public static bool EstaDesbloqueada(string personagem)
    {
        return PlayerPrefs.GetInt(
            "Figurinha_" + personagem, 0
        ) == 1;
    }

    public static void Resetar()
    {
        string[] personagens = {
            "DonaBiscoito",
            "CapitaoBigode",
            "Luna",
            "Zeca"
        };

        foreach (string personagem in personagens)
        {
            PlayerPrefs.DeleteKey("Figurinha_" + personagem);
        }

        PlayerPrefs.Save();
    }
}
