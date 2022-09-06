using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Adiciona a bibliotecade User Interface
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{	
	//Variaveis privadas	
	float horizontal;	
	bool jump = false;
	bool canControlPlayer = true;	
	Rigidbody2D rb;
	SpriteRenderer spr;
	AudioSource audioS;
	float nextAttackTime = 0f;
	int currentHealth;
	int maxAgua = 3;
	int maxPower = 3;
	int currentAgua = 0;
	int currentPower = 0;
	bool left = false;
	float enemyCheckX = 0f;
	float enemyCheckY = 0f;
	float firePointX = 0f;
	float firePointY = 0f;
	float isJumping;
	bool pegouCruz = false;
	bool dead = false;
	float nextFireballTime = 0f;
	float fireballRate = 1f;
	bool isAttacking = false;
	
    //Variaveis publicas
	public float playerSpeed;
	public float jumpForce;
	public GameObject itemFeedback;
	public GameObject enemyDeathPF;
	public AudioClip jumpSFX, pickupSFX, enemyDeathSFX, playerDeathSFX, laserBeamSFX, fireBallSFX, playerHurtSFX, playerAttackSFX, enemyGotHurtSFX, healingSpellSFX, bossDeathSFX;
	public Animator anim;
	public Transform attackPoint;
	public Transform firePoint;
	public Transform player;
	public float attackRange = 1.2f;
	public LayerMask enemyLayers;
	public LayerMask bossLayer;
	public int attackDamage = 50;
	public float attackRate;
	public int maxHealth = 100;
	public Text lifeCounterText;
	public Text aguaCounterText;
	public Text powerCounterText;
	public float difRayY = -1.6f;
	public float difRayX = -0.6f;
	public GameObject bulletPrefab;
	public Transform cam;

	// Start is called before the first frame update aaa
	void Start()
    {
		dead = false;
		playerSpeed = 5f; // IEnumerator Slow() seta speed tb
		//Inicialização de Variáveis
		rb = GetComponent<Rigidbody2D>();
		//rb = this.GetComponent<Rigidbody2D>();
		//Atribuindo a scene atual para a variavel 'scn'
		spr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		audioS = GetComponentInChildren<AudioSource>();
		currentHealth = maxHealth;
		currentAgua = maxAgua; 
		currentPower = maxPower;
		aguaCounterText.text = currentAgua.ToString("0") + "/" + maxAgua.ToString("0");
		powerCounterText.text = currentPower.ToString("0") + "/" + maxPower.ToString("0");
		lifeCounterText.text = currentHealth.ToString("000") + "/" + maxHealth.ToString("000");
		
		
	}


    // UPDATE is called once per frame
    void Update()
    {

		//Verifica se o player pode controlar o personagem
		if (canControlPlayer==true && isAttacking == false)
		{	
		//Leitura do eixo Horizontal e atribuição na variável
		horizontal = Input.GetAxis("Horizontal");
		//Atribui a velocidade x do Rigidbody2D o valor da leitura do eixo horizontal * speed
		rb.velocity = new Vector2(playerSpeed * horizontal, rb.velocity.y);		
		}
		
		// JUMP  Verifica se o Player pressionou o botão 'Jump' e se a velocidade y está zerada
		if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.H))
		{
			if(rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
			{
			//Atribui true a variavel jump
			jump = true;
			// toca o audio
			audioS.PlayOneShot(jumpSFX, 0.1f);
			}
		}			
		isJumping = anim.GetFloat("speedY");

		//ATAQUE
		if (Time.time >= nextAttackTime && isJumping > -0.1 && isJumping < 0.1 && dead == false)
        {
			//chama a funcao ataque quando a tecla 'Q' é precionada
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.J))
			{				
				Attack();
				nextAttackTime = Time.time + 1f / attackRate;
			}
		}

		//CURA 
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.K))
		{ 
			if(currentAgua > 0 && currentHealth < maxHealth && dead == false)
			{
				audioS.PlayOneShot(healingSpellSFX, 0.3f);
				currentHealth = maxHealth;
				anim.SetTrigger("Healing");
				currentAgua = currentAgua - 1;
				lifeCounterText.text = currentHealth.ToString("000") + "/" + maxHealth.ToString("000");
				aguaCounterText.text = currentAgua.ToString("0") + "/" + maxAgua.ToString("0");
			} 
		}

		//FIREBALLL
		if (Time.time >= nextFireballTime && dead == false)
		{
			if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.L))
			{
				if (currentPower > 0)
				{
					Shoot();
					currentPower = currentPower - 1;
					powerCounterText.text = currentPower.ToString("0") + "/" + maxPower.ToString("0");
					nextFireballTime = Time.time + 1f / fireballRate;
				}
			}
		}
			
		//Setando os parametros do Animator do player 
		anim.SetFloat("speed", Mathf.Abs(horizontal)); 
		anim.SetFloat("speedY", rb.velocity.y);
		
		//Executar o flip do sprite do player
		if(horizontal < 0 )
		{
			spr.flipX = true;
			Debug.Log("left");
			left = true;
		}
		else
				{
					if(horizontal > 0)
					{
						spr.flipX = false;
						Debug.Log("right");
						left = false;
					}
				}	
	}
	
	//Função FixedUpdate é usada para simulações
	private void FixedUpdate()
	{
		//Se 'Jump' for verdadeiro	
		if(jump == true)
		{

			//Atribui a força no Rigidbody2D do player	
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);			

			// Jump se torna falso
			jump = false;

		}

		// diferença do y do inimigo para o y onde deve ficar a bolinha do raycheck
		enemyCheckY = player.transform.position.y - difRayY;
		firePointY = player.transform.position.y;
	
		if (left == true)
        {
			enemyCheckX = player.transform.position.x - difRayX;
			attackPoint.transform.position = new Vector3(enemyCheckX, enemyCheckY, 0);
			
			firePointX = player.transform.position.x - 1.3f;
			firePoint.transform.position = new Vector3(firePointX, firePointY, 0);

			firePoint.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
		}
		else
        {
			enemyCheckX = player.transform.position.x + difRayX;
			attackPoint.transform.position = new Vector3(enemyCheckX, enemyCheckY, 0);
			
			firePointX = player.transform.position.x + 1.3f;
			firePoint.transform.position = new Vector3(firePointX, firePointY, 0);

			firePoint.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		
	}




	void Shoot()
	{
		// toca o audio
		audioS.PlayOneShot(fireBallSFX, 0.3f);
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}

	public void PlayEnemyGotHurt()
	{
		audioS.PlayOneShot(enemyGotHurtSFX, 0.3f);
	}

	//funcao para atacar
	void Attack()
	{
		audioS.PlayOneShot(playerAttackSFX, 0.3f);
		
		anim.SetTrigger("Attack");

		StartCoroutine(Slow());

		Collider2D[] Boss = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayer);

		foreach (Collider2D hitBoss in Boss)
		{
			hitBoss.GetComponent<Boss>().TakeDamage(attackDamage);
		}

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
		}		
	}

	public void PlayerTakeDamage(int damage)  //player toma damage aa a aa
	{
		audioS.PlayOneShot(playerHurtSFX, 0.2f);
		currentHealth -= damage;
		
		if (currentHealth>0)
		{
			lifeCounterText.text = currentHealth.ToString("000") + "/" + maxHealth.ToString("000");
			anim.SetTrigger("Hurt");
		}
        else
        {
			currentHealth = 0;
			lifeCounterText.text = currentHealth.ToString("000") + "/" + maxHealth.ToString("000");
			//Chama a coroutine PlayerDeath	
			StartCoroutine(PlayerDeath());
		}
	}

	IEnumerator Slow()
	{
		playerSpeed = 1f;
		isAttacking = true; 
		yield return new WaitForSeconds(0.3f);
		isAttacking = false;
		playerSpeed = 5f;
	}

	void OnDrawGizmosSelected()
	{
		if (attackPoint == null )
		
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);		
	}


    public void PlayLaserAudio() 
	{
		audioS.Stop();
		audioS.loop = true;
		audioS.clip = laserBeamSFX;
		audioS.volume = 0.5f;
		audioS.Play();
	}


	public void PlayBossDeathAudio()
	{
		audioS.PlayOneShot(bossDeathSFX);
		cam.GetComponent<StopMusic>().StopBattleMusic();
	}

	public void StopLaserAudio()
	{
		audioS.volume = 1f;
		audioS.Stop();
		audioS.loop = false;
	}

	//Função de colisão do tipo Trigger
	private void OnTriggerEnter2D(Collider2D collision)
	{	
		//se colidir com um objeto com a tag "pickup Gem" aaa
		if (collision.CompareTag("Pickup Gem"))
		{
			//Chama a função AddGem que está declarada no Game Manager
			GameManager.gm.AddGem();
			//destroi o objeto com o qual o player colidiu			 
			 Destroy(collision.gameObject);
			 //Instancia um objeto "itemFeedback" na posição e na rotação da colisão
			 //Instantiate(itemFeedback, collision.transform.position, collision.transform.rotation);
			 // toca o audio
			 //audioS.PlayOneShot(pickupSFX);
		}

		if (collision.CompareTag("Agua"))
		{
			if (currentAgua < maxAgua)
			{
				currentAgua = maxAgua;
				
				aguaCounterText.text = currentAgua.ToString("0") + "/" + maxAgua.ToString("0");
				pegouCruz = true;				
			}
			if (currentPower < maxPower)
			{
				currentPower = maxPower;

				powerCounterText.text = currentPower.ToString("0") + "/" + maxPower.ToString("0");
				pegouCruz = true;
			}
			if(pegouCruz==true)
			{				
				//destroi o objeto com o qual o player colidiu				
				Destroy(collision.gameObject);
				pegouCruz = false;
			}
		}
			//Se colidir com um objeto com a tag "Enemy"
			if (collision.CompareTag("Enemy"))
		{	
			//Chama a coroutine PlayerDeath
			StartCoroutine(PlayerDeath());
		}			
	}
	
	//Declaração da coroutine PlayerDeath(morte do player)
	IEnumerator PlayerDeath()
	{
		
		//aciona o trigger da animação de morte no animator		 
		anim.SetBool("isDead", true);	  
		anim.SetTrigger("DEAD");
		dead = true;
		//yield return new WaitForSeconds(0.3f);   
		//roda o audio  
		//audioS.PlayOneShot(playerDeathSFX);
		//audioS.PlayOneShot(playerDeathSFX);
		currentHealth = 0;
		lifeCounterText.text = currentHealth.ToString("000") + "/" + maxHealth.ToString("000");
		//Desabilita o controle do player
		canControlPlayer = false;
		//zera a velocidade do rigidbody
		rb.velocity = Vector2.zero;
		//troca o tipo do rigidbody para 0
		rb.isKinematic = true;
		//desliga o componente capsuleCollider do player
		GetComponent<CapsuleCollider2D>().enabled = false;
		
		//Pausa na coroutine de 2.5 segundos
		yield return new WaitForSeconds(2.5f);
		//chama a função ReloadScene do Game Manager
		GameManager.gm.ReloadScene();
	}	
	
	 /*IEnumerator ExecuteAfterTime(float time)
		 {
			 yield return new WaitForSeconds(time);
		 
			 // Code to execute after the delay
		 }*/
	
	/*
	//Função de colisão do tipo "não trigger"	
	private void OnCollisionEnter2D(Collision2D collision)
	{	
		//Detecta se o player colidiu com um objeto que possui a tag "Enemy"
		if(collision.gameObject.CompareTag("Enemy"))
		{
			//atribui true a variavel jump
			jump = true;
			//Destroi o objeto com o qual o player colidiu
			Destroy(collision.gameObject);
			//roda o audio
			audioS.PlayOneShot(enemyDeathSFX, 0.2f);
			//Instancia um objeto "itemFeedback" na posição e na rotação da colisão
			Instantiate(enemyDeathPF, collision.transform.position, collision.transform.rotation);
		}
	}
	*/
}