using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
	// Start is called before the first frame update
	public AudioSource audioS;

    public void StopBattleMusic()
	{ 
		StartCoroutine(StopBattleSound());
    }


	IEnumerator StopBattleSound()
	{

		yield return new WaitForSeconds(0.3f);
		audioS.volume = 0.6f;
		yield return new WaitForSeconds(0.5f);
		audioS.volume = 0.1f;
		yield return new WaitForSeconds(0.5f);
		audioS.loop = false;
		audioS.Stop();

	}
}