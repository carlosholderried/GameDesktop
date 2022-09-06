using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
	//Variaveis privadas
	RaycastHit2D groundInfo;  // nao sei se precisa
	int currentHealth;
	private Rigidbody2D rb;
	private Vector2 movement;
	private float playerDistance;
	float nextAttackTime = 0f;      // sei la
	bool left = true;
	float edgeCheckX = 0f;
	float edgeCheckY = 0f;
	bool isHurt = false;
	bool moving = true;
	bool dead = false;
	bool active = false;

	//Variaveis publicas
	public float moveSpeed;
	public int maxHealth = 100;
	public Animator anim;
	public Transform player;
	public Transform edgeCheckT;
	public Transform enemy;
	public SpriteRenderer spr; //dss
	public GameObject edgeCheck;
	public float rayDistance = 1;
	public int attackDamage = 50;
	public float attackRate = 0.7f;
	public float difRayY = -0.4f; //cada inimigo seta seu proprio la na unity
	public float difRayX = 1f; //cada inimigo seta seu proprio la na unity
	public float enemyRange;
	public BoxCollider2D childBC2D;
	public GameObject daggerPrefab;
	public Transform throwPoint;
	public Transform gm;
	//public GameObject spearPrefab;  

    // FUNCAO QUE RODA 1X NO INICIO 
    void Start()
	{	

		currentHealth = maxHealth;
		rb = this.GetComponent<Rigidbody2D>();
		
			StartCoroutine(ThrowDagger());
		
	}

	// Update is called once per frame
	void Update()
	{
		if (player.position.y > 10f && dead == false)
		{
			//ground.GetComponent<SkellyCollider>().DisableCollider();
			if(active==false)
			{
				StartCoroutine(StopCollider());
			}
		}
			//SE FOR BANDIDO OU ESQUELETO
			if (enemy.CompareTag("Bandit") || enemy.CompareTag("Skeleton"))
		{
			//DEFINE A DIREÇÃO EM QUE VAI SE MOVIMENTAR
			Vector3 direction = player.position - transform.position;
			direction.Normalize();
			movement = direction;

			//RESGATA A INFORMAÇÃO DE QUE O PLAYER ESTA MORTO OU NAO
			bool isDeadTemp = player.GetComponent<Animator>().GetBool("isDead");

			//ATACA O PLAYER CASO NÃO TENHA TOMADO HIT E O PLAYER ESTEJA VIVO E PERTO DO INIMIGO e player nao ta pulando
			if (player.position.y < 10f && playerDistance <= enemyRange && isDeadTemp == false && isHurt == false)               // ataca o player aa aaaaaaa
			{
				StartCoroutine(RealAttack());
				//FUNCAO PARA STAGGER DO INIMIGO
				IEnumerator RealAttack()
				{
					//ESPERA 0.3 SEGUNDOS					
					yield return new WaitForSeconds(0.3f);
					//difY = Mathf.Abs(enemy.position.y-player.position.y); nao funfou
					//ATACA O PLAYER CASO ELE ESTEJA PERTO, NAO ESTEJA PULANDO, NAO ESTEJA MORTO E O ATACANTE NAO TOMOU HIT RECENTEMENTE
					if (Time.time >= nextAttackTime && player.position.y < 10 && playerDistance <= enemyRange && isDeadTemp == false && isHurt == false)
					{
						StartCoroutine(AttackAaa());
						nextAttackTime = Time.time + 1f / attackRate;
					}

				}


			}
		}
	}


	public void FixedUpdate()
	{
		//SE FOR BANDIDO OU ESQUELETO
		if (enemy.CompareTag("Bandit") || enemy.CompareTag("Skeleton"))
		{
			playerDistance = Mathf.Abs(enemy.transform.position.x - player.transform.position.x);

			//Variavel "groundInfo" recebe as informações de um raycast que "monitora" o chão qs aaaaaa
			groundInfo = Physics2D.Raycast(edgeCheck.transform.position, Vector2.down, rayDistance);
			// diferença do y do inimigo para o y onde deve ficar a bolinha do raycheck
			edgeCheckY = enemy.transform.position.y - difRayY;

			//SE ESTIVER VIRADO PARA A ESQUERDA MOVE O EDGECHECK PARA ESQUERDA DO PERSONAGEM
			if (left == true)
			{
				edgeCheckX = enemy.transform.position.x - difRayX;
				edgeCheckT.transform.position = new Vector3(edgeCheckX, edgeCheckY, 0);

				if (enemy.CompareTag("Skeleton"))
				{
					throwPoint.transform.position = new Vector3(enemy.transform.position.x - 2f, enemy.transform.position.y, 0f);
				}
				else
				{
					throwPoint.transform.position = new Vector3(enemy.transform.position.x - 1f, enemy.transform.position.y + 1.5f, 0f);
				}
				//SE ESTIVER VIRADO PARA A DIREITA MOVE O EDGECHECK PARA DIREITA DO PERSONAGEM
			}
			else
			{
				edgeCheckX = enemy.transform.position.x + difRayX;
				edgeCheckT.transform.position = new Vector3(edgeCheckX, edgeCheckY, 0);

				if (enemy.CompareTag("Skeleton"))
				{
					throwPoint.transform.position = new Vector3(enemy.transform.position.x + 2f, enemy.transform.position.y, 0f);
				}
				else
				{
					throwPoint.transform.position = new Vector3(enemy.transform.position.x + 1f, enemy.transform.position.y + 1.5f, 0f);
				}
			}

			//SE O PLAYER ESTIVER PERTO, HÁ CHÃO E O ATACANTE NÃO TOMOU HIT RECENTEMENTE
			if (playerDistance > enemyRange && playerDistance < 10 && groundInfo.collider == true && isHurt == false && moving == true)
			{
				//CHAMA FUNCAO DE MOVIMENTO
				moveCharacter(movement);
				//BOOLEAN INIMIGO SE MOVENDO TRUE
				anim.SetBool("enemyMoving", true);
			}
			else
			{
				//BOOLEAN INIMIGO SE MOVENDO FALSO 
				anim.SetBool("enemyMoving", false);
			}

			//SE FOR UM BANDIDO
			if (enemy.CompareTag("Bandit"))
			{
				//VIRA A SPRITE PARA A ESQUERDA SE O PLAYER ESTIVER A ESQUERDA  aa
				if (enemy.transform.position.x > player.transform.position.x) //fdgj
				{
					//inverte a direção que o inimigo anda(sprite only)
					spr.flipX = false;
					//Quaternion rotation = Quaternion.Euler(0, 0, 0);
					//enemy.transform.rotation = rotation;
					// inverte X do edgeCheck
					left = true;
				}
				//VIRA A SPRITE PARA A DIREITA SE O PLAYER ESTIVER A DIREITA
				else
				{
					//inverte a direção que o inimigo anda(sprite only)
					spr.flipX = true;
					// inverte X do edgeCheck
					//Quaternion rotation = Quaternion.Euler(0, 180, 0);
					//enemy.transform.rotation = rotation;
					left = false;
				}
			}

			//SE FOR UM ESQUELETO
			if (enemy.CompareTag("Skeleton"))
			{
				//VIRA A SPRITE PARA A ESQUERDA SE O PLAYER ESTIVER A ESQUERDA
				if (enemy.transform.position.x > player.transform.position.x)
				{
					//inverte a direção que o inimigo anda(sprite only)
					spr.flipX = true;
					// inverte X do edgeCheck
					left = true;

				}
				//VIRA A SPRITE PARA A DIREITA SE O PLAYER ESTIVER A DIREITA aaaa
				else
				{
					//inverte a direção que o inimigo anda(sprite only)
					spr.flipX = false;
					// inverte X do edgeCheck
					left = false;
				}

			}
		}
	}
	

	//FUNCAO DE ATAQUE
	IEnumerator AttackAaa()
	{
		anim.SetTrigger("Attack");
		yield return new WaitForSeconds(0.4f);
		if (isHurt == false)
		{
			player.GetComponent<PlayerScript>().PlayerTakeDamage(attackDamage);
		}
	}
	
	//TACA ADAGA
	IEnumerator ThrowDagger()
	{
		if (playerDistance < 9 && dead == false)
		{
			StartCoroutine(StopMov());
			yield return new WaitForSeconds(3f);
		}   
		yield return new WaitForSeconds(1f);
		StartCoroutine(ThrowDagger());
	}

	void Throw()
	{
     //   if(enemy.CompareTag("Skeleton"))
	//	{
			//Instantiate(spearPrefab, throwPoint.position, throwPoint.rotation);
	//	}
      //  else
		//{
			Instantiate(daggerPrefab, throwPoint.position, throwPoint.rotation);
	//	}
		
	}

	IEnumerator StopMov()
	{
		moving = false;
		yield return new WaitForSeconds(0.3f);
		Throw();
		yield return new WaitForSeconds(0.4f);
		moving = true;
	}

	//FUNCAO PARA O INIMIGO TOMAR DANO
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		StartCoroutine(Stagger());

		player.GetComponent<PlayerScript>().PlayEnemyGotHurt();

		anim.SetTrigger("Hurt");

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	IEnumerator StopCollider()
	{
		active = true;
		childBC2D.enabled = false;
		yield return new WaitForSeconds(1f);
		childBC2D.enabled = true;
		active = false;
		if (dead==true) 
		{
			childBC2D.enabled = false;

		}
	}
	
	
	//FUNCAO PARA STAGGER DO INIMIGO
	IEnumerator Stagger()
	{
			isHurt = true;
			yield return new WaitForSeconds(0.5f);
			isHurt = false;
	}		
	
	//FUNCAO DE MORTE DO INIMIGO
	void Die()
	{
		
		anim.SetBool("isDead", true);

		dead = true;

		GetComponent<Collider2D>().enabled = false;

		childBC2D.enabled = false;

		gm.GetComponent<GameManager>().KillCount();

        EnemyScript enemyScript = this;
        enemyScript.enabled = false;
	}
	
	//FUNCAO PARA MOVIMENTO
	void moveCharacter(Vector2 direction)
    {
		rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
	}
	//Usamos a função Translate para mover o inimigo	
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
	
	
}
