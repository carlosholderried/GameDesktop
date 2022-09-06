using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
	public float speed = 10f;
	public Rigidbody2D rb;
	public int damage = 30;
		
	// Start is called before the first frame update
	void Start()
	{
		rb.velocity = transform.right * speed;
		StartCoroutine(DestroySpear());
	}
		
	// Update is called once per frame a
	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		PlayerScript player = hitInfo.GetComponent<PlayerScript>();
		if (player != null)
		{
			player.PlayerTakeDamage(damage);
			Destroy(gameObject);
		}
	}
		
	IEnumerator DestroySpear()
	{
		yield return new WaitForSeconds(0.7f);
		Destroy(gameObject);
	}
}
