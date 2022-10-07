using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //variaveis publicas
    public GameObject mainMenu, levelSelect;
    public Button[] lvlButtons;

    // Start is called before the first frame update
    void Start()
    {
        //chama o menu principal
        MainMenu();
    }

    //reseta fases
    public void ResetLevels()
    {
       PlayerPrefs.DeleteAll();
            CheckLevels();       
    }

    //funcao para acessar o menu principal 
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
    }

    //funcao para acessar o menu de seleção de fases
    public void LevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);
        CheckLevels();
    }

    //funcao para carregar uma fase
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    //funcao para habilitar/desabilitar as fases do menu de seleção defases
    void CheckLevels()
    {   
        //percorre o vetor de botoes     
        for(int i=1; i<lvlButtons.Length; i++) 
        {
            //verifica se existe uma chave gravada no PlayerPrefs
            if (PlayerPrefs.HasKey("Level" + (i + 1).ToString() + "Unlocked")) 
            {
                //ativa o botao
                lvlButtons[i].interactable = true;
            }
            else
            {   
                //desativa o botao
                lvlButtons[i].interactable = false;
            }

        }
    }

    //funcao para sair do jogo(funciona somente para PC) 
    public void QuitGame()
    {
        Application.Quit();
    }
}
