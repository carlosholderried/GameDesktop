using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float attackRate = 10f;
    float nextAttackTime = 0f;
    float currentHealth = 1000;
    private Rigidbody2D rb;
    int cont = 20;

    public Transform leftFirePoint1;
    public Transform leftFirePoint2;
    public Transform midFirePoint1;
    public Transform midFirePoint2;
    public Transform rightFirePoint1;
    public Transform rightFirePoint2;
    public Transform boss;
    public BoxCollider2D boxCollider2D;
	public Transform ground;
	public Transform player;
	public GameObject spykesPrefab;
	public Transform spykesPoint;
    public Transform hearts;
    public Transform bossPic;
	

    public Animator anim;

    public float maxHealth = 1000;

    public int laserDamage = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LaserBoss());
		StartCoroutine(SpykesBoss());
        currentHealth = maxHealth;
        rb = this.GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {      

        if (Time.time >= nextAttackTime)
        {
            if (anim.GetBool("leftLaser") == true)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(leftFirePoint1.position, leftFirePoint1.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

                RaycastHit2D hitInfo2 = Physics2D.Raycast(leftFirePoint2.position, leftFirePoint2.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo2.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

            }


            if (anim.GetBool("midLaser") == true)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(midFirePoint1.position, midFirePoint1.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

                RaycastHit2D hitInfo2 = Physics2D.Raycast(midFirePoint2.position, midFirePoint2.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo2.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

            }


            if (anim.GetBool("rightLaser") == true)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(rightFirePoint1.position, rightFirePoint1.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

                RaycastHit2D hitInfo2 = Physics2D.Raycast(rightFirePoint2.position, rightFirePoint2.right);
                if (hitInfo)
                {
                    PlayerScript player = hitInfo2.transform.GetComponent<PlayerScript>();
                    if (player != null)
                    {
                        player.PlayerTakeDamage(laserDamage);
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }

            }
        }
    }

    //FUNCAO PARA O INIMIGO TOMAR DANO
    public void TakeDamage(int damage)
    {
        cont = cont - 1;
        hearts.GetComponent<Hearts>().checkHearts(cont);
        player.GetComponent<PlayerScript>().PlayEnemyGotHurt();
        currentHealth -= damage;
        anim.SetTrigger("gotHurt");

        if (currentHealth <= 0)
        {
            StartCoroutine(BossDeath());
        }
    }

    IEnumerator BossDeath()
    {

        ground.GetComponent<GroundOnOff>().BarrierOff();

        player.GetComponent<PlayerScript>().PlayBossDeathAudio();

        anim.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;

        bossPic.GetComponent<BossPic>().DestroyBossPic();

        yield return new WaitForSeconds(0.8f);

        Destroy(gameObject);

        this.enabled = false;
    }
	
			IEnumerator SpykesBoss()
		{			
			yield return new WaitForSeconds(5f);
			spykesPoint.transform.position = new Vector3(player.transform.position.x, 7.3f, player.transform.position.z);	
			yield return new WaitForSeconds(1f);
			Instantiate(spykesPrefab, spykesPoint.position, spykesPoint.rotation);
			StartCoroutine(SpykesBoss());	
		}

    IEnumerator LaserBoss()
    {
 
            yield return new WaitForSeconds(3f); //IMPORTANTE PQ SIM

        if (anim.GetBool("isDead") == false)
        {
            boxCollider2D.enabled = false;
            anim.SetTrigger("b4MidLaser");
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("midLaser", true);
            player.GetComponent<PlayerScript>().PlayLaserAudio();
            yield return new WaitForSeconds(2);//TEMPO QUE O LASER FICA ATIVO
            anim.SetBool("midLaser", false);
            player.GetComponent<PlayerScript>().StopLaserAudio();
            boxCollider2D.enabled = true;
        }

            yield return new WaitForSeconds(0.1f);

        if (anim.GetBool("isDead") == false)
        {
            boxCollider2D.enabled = false;
            anim.SetTrigger("b4LeftLaser");
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("leftLaser", true);
            player.GetComponent<PlayerScript>().PlayLaserAudio();
            yield return new WaitForSeconds(2);//TEMPO QUE O LASER FICA ATIVO
            anim.SetBool("leftLaser", false);
            player.GetComponent<PlayerScript>().StopLaserAudio();
            boxCollider2D.enabled = true;
        }

            yield return new WaitForSeconds(0.1f);

        if (anim.GetBool("isDead") == false)
        {
            boxCollider2D.enabled = false;
            anim.SetTrigger("b4RightLaser");
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("rightLaser", true);
            player.GetComponent<PlayerScript>().PlayLaserAudio();
            yield return new WaitForSeconds(2);//TEMPO QUE O LASER FICA ATIVO
            anim.SetBool("rightLaser", false);
            player.GetComponent<PlayerScript>().StopLaserAudio();
            boxCollider2D.enabled = true;
        }
			yield return new WaitForSeconds(2f); //IMPORTANTE PQ SIM
            
            StartCoroutine(LaserBoss());
            
        
    }


}
