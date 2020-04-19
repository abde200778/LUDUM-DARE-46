using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huntrox.Games.LD46;
using UnityEngine.SceneManagement;
public class AnimatorEvents : MonoBehaviour
{

	public void GameStart()
	{
		SceneManager.LoadSceneAsync(1);
	}

	public void PlayPaddlesSound()
	{

		// this method is called  by paddle animation
		AudioManager.inctanse.PlayOneshot(SFX_TYPE.Paddles_SFX,0);


	}
}
