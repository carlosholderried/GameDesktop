using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 10f;
	public Rigidbody2D rb;
	public int damage = 50;

	// Start is called before the first frame update
	void Start()
	{
		rb.velocity = transform.right * speed;
		StartCoroutine(DestroyBullet());
	}

	// Update is called once per frame a
	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		EnemyScript enemy = hitInfo.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}
		Destroy(gameObject);

		Boss boss = hitInfo.GetComponent<Boss>();
		if (boss != null)
		{
			boss.TakeDamage(damage);
		}
		Destroy(gameObject);
	}

	IEnumerator DestroyBullet() 
	{
		yield return new WaitForSeconds(0.69f);
		Destroy(gameObject);
	}


}
