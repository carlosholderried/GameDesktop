using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Adiciona a bibliotecade User Interface
using UnityEngine.UI;
//Adiciona a bibliotecade de gerencionamento de cenas(scenes)
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{	
	//Variaveis privadas
	int gems, totalGems = 0;
	int killedEnemies, totalEnemies = 0;
	Scene scn;
	bool isPaused = false;
	
	//Variaveis publicas
	public static GameManager gm;
	public Text gemCounterText;
	public GameObject pausePanel, endLevelPanel;
	public Transform barrier;
	

    // Start is called before the first frame update
    void Start()
    {
		//inicializando a variavel gm com a atribuição deste objeto(o proprio objeto)	
		gm = this;
		//Chamar a função CountTotalGems para a contagem de gemas
		CountTotalGems();
		//Atualiza o text da interface
        gemCounterText.text = gems.ToString("00") + "/" + totalGems.ToString("00");
		//Atribuindo a scene atual para a variavel 'scn'
		scn = SceneManager.GetActiveScene();
		//garantir que os paineis de pausa e de fim de fase estejam desativados
		pausePanel.SetActive(false);
		endLevelPanel.SetActive(false);
		//rodar o game na velocidade normal
		Time.timeScale = 1;
		CountTotalEnemies();
	}
    
	private void Update()
	{
		//codigo para pausar o jogo
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}

		if (killedEnemies == totalEnemies && scn.buildIndex != 3)
		{
			
			barrier.GetComponent<Barrier>().BarrierOff();
		}

	}		
	
	//Função para ser chamada quando o Player coletar uma gem
    public void AddGem()
    {
        //Incrementa a variavel get
		gems = gems + 1;
		//Atualiza o text na interface
		gemCounterText.text = gems.ToString("00") + "/" + totalGems.ToString("00");
		//Verifica se o player coletou todas as gems
		if (gems >= totalGems)
		{
			//grava a chave para habilitar a proxima fase no menu de seleção de fases
			PlayerPrefs.SetInt("Level" + (scn.buildIndex + 1).ToString() + "Unlocked", 1);
			LoadNextLevel();
			//PAUSA o jogo
			Time.timeScale = 0;
		}
    }

	//Função para carregar o menu principal
	public void QuitToMenu()
	{
		SceneManager.LoadScene(0);
	}

	//Função para reinciar a faase (scene)
	public void ReloadScene()
	{
		//Função da unity para carregar uma scene
		SceneManager.LoadScene(scn.buildIndex);
	}
	
	//função para carregar a proxima fase
	public void LoadNextLevel()
    {
		//verifica se a fase é a ultima, dai carrega o menu principal
		if (scn.buildIndex == 3)
		{
			Time.timeScale = 0;
			SceneManager.LoadScene(4);
		}
		else
		{
			//carrega a proxima fase
			SceneManager.LoadScene(scn.buildIndex + 1);
		}
    }

	//Função para contagel do total de gemas no inicio da fase
	void CountTotalGems()
	{
		//Cria um vetor de gameObjects que recebe todos os objetos com a tag "pickup Gem"
		GameObject[] gems = GameObject.FindGameObjectsWithTag("Pickup Gem");
		//Percorremos o vetor e adicionamos 1 na variavel totalGems para cada objeto do vetor
		foreach (GameObject g in gems)
		{
			//Incrementa a variavel totalGems
			totalGems++;
		}
	}

	void CountTotalEnemies()
	{
		//Cria um vetor de gameObjects que recebe todos os objetos com a tag "pickup Gem"
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Bandit");
		//Percorremos o vetor e adicionamos 1 na variavel totalGems para cada objeto do vetor
		foreach (GameObject g in enemies)
		{
			//Incrementa a variavel totalGems
			totalEnemies++;
		}

		GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Skeleton");
		//Percorremos o vetor e adicionamos 1 na variavel totalGems para cada objeto do vetor
		foreach (GameObject g in enemies2)
		{
			//Incrementa a variavel totalGems
			totalEnemies++;
		}

	}

	public void KillCount()
	{
		//Incrementa a variavel get
		killedEnemies = killedEnemies + 1;
	}

		//Função que pausa o game
		public void PauseGame()
	{
		//verifica a variavel isPaused	
		if(isPaused == false)
		{
			//Pausar o jogo
			isPaused = true;
			Time.timeScale = 0;
			pausePanel.SetActive(true);
		}
		else
		{
			//Despausar o jogo
			isPaused = false;
			Time.timeScale = 1;
			pausePanel.SetActive(false);
		}			
	}	
	
}
