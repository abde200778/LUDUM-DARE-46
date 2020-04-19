using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace Huntrox.Games.LD46
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager inctanse;

		//public AudioMixer SFX_audioMixer;
		//public AudioMixer Music_audioMixer;
		//public AudioMixer General_audioMixer;
		private AudioSource _audioSource;

		public AudioClip[] SFX;
		public AudioClip[] paddlesSFX;
		void Awake() => inctanse = this;

		void Start()
		{
			_audioSource = GetComponent<AudioSource>();
		}


		public void PlayOneshot(SFX_TYPE sfx_type,float delay)
		{
		StartCoroutine(PlaySFX(sfx_type, delay));

		}

		private IEnumerator PlaySFX(SFX_TYPE sfx_type,float delay)
		{
			yield return new WaitForSeconds(delay);
			switch (sfx_type)
			{
				case SFX_TYPE.button_click:
					_audioSource.PlayOneShot(SFX[0]);
					break;
				case SFX_TYPE.Hit_SFX:
					_audioSource.PlayOneShot(SFX[1]);
					break;
				case SFX_TYPE.Miss_SFX:
					_audioSource.PlayOneShot(SFX[2]);
					break;
				case SFX_TYPE.Coins_SFX:
					_audioSource.PlayOneShot(SFX[0]);
					break;
				case SFX_TYPE.Paddles_SFX:
					_audioSource.PlayOneShot(paddlesSFX[Random.Range(0, paddlesSFX.Length)]);
					break;
				case SFX_TYPE.Popup_SFX:
					_audioSource.PlayOneShot(SFX[3]);
					break;

			}
		}
	}



	public enum SFX_TYPE {button_click,Hit_SFX,Miss_SFX,Coins_SFX ,Paddles_SFX,Popup_SFX}
}