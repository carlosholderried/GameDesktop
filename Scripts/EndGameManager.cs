using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    //variaveis publicas
    public GameObject endGame;

    // Start is called before the first frame update
    void Start()
    {
        //chama o menu principal
        EndGame();
    }

    //funcao para acessar o menu principal 
    public void EndGame()
    {
        endGame.SetActive(true);
    }

    //funcao para sair do jogo(funciona somente para PC) h
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
