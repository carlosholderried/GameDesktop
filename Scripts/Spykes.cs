using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spykes : MonoBehaviour
{
	
	public float oldX;
	public Rigidbody2D rb;
	public int damage = 50;
	public BoxCollider2D boxCollider2D;
	public Transform spykes;
	public Transform player;


	// Start is called before the first frame update
	void Start()
	{
		oldX = spykes.transform.position.x;
		StartCoroutine(DestroyBullet());
	}

	// Update is called once per frame a aa
	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		PlayerScript player = hitInfo.GetComponent<PlayerScript>();
		if (player != null)
		{
			boxCollider2D.enabled = false;
			player.PlayerTakeDamage(damage);
		}
	}

	IEnumerator DestroyBullet() 
	{
		yield return new WaitForSeconds(0.1f);
		spykes.transform.position = new Vector3(oldX, 7.6f, player.transform.position.z);
		yield return new WaitForSeconds(0.1f);
		spykes.transform.position = new Vector3(oldX, 7.9f, player.transform.position.z);
		yield return new WaitForSeconds(0.6f);

		Destroy(gameObject);
	}
}
