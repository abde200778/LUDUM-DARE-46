
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
namespace Huntrox.Games.LD46 {
	public class Settings : MonoBehaviour
	{
		// Start is called before the first frame update
		public static Settings instance;

		public AudioMixer Master;

		//public Dropdown ScreanSize;
		public TMP_Dropdown ScreanSize;
		public Slider VolumeSlid;
		public Slider sfxSlid;
		public Slider musicSlid;
		public Toggle Fullscreen;
		Resolution[] resolutions;
		public GameObject settings;
		private void Awake()
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		private void Start()
		{

			resolutions = Screen.resolutions;
			ScreanSize.ClearOptions();
			List<string> options = new List<string>();
			int CurrIndex = 0;
			for (int i = 0; i < resolutions.Length; i++)
			{
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);
				if (resolutions[i].width == Screen.width &&
					resolutions[i].width == Screen.width)
				{

					CurrIndex = i;
				}

			}

			ScreanSize.AddOptions(options);
			ScreanSize.value = CurrIndex;
			ScreanSize.RefreshShownValue();

		}
		public void SetMasterVolume( )
		{

			Master.SetFloat("MasterVolume", VolumeSlid.value);

		} 
  		public void SetSFXVolume()
		{ 
			Master.SetFloat("SFXVolume",sfxSlid.value);

		} 
    	public void SetMusicVolume( )
		{

			Master.SetFloat("SoundTrackVolume", musicSlid.value);
			Master.SetFloat("BGVolume", musicSlid.value);

		}

		//public void SetFullScrean()
		//{

		//	Screen.fullScreen = value;

		//}
		public void ChangeResolution( )
		{

			Resolution resolution = resolutions[ScreanSize.value];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

		}
		public void StarGAme()
		{

			SceneManager.LoadSceneAsync(1);

		} public void QuitGame()
		{

			Application.Quit();

		}
		public void OpenSetting()
		{

			GameObject.FindGameObjectWithTag("GM").GetComponent<UI_popup>().EnableObject(settings, 0.3f, true);

		}
		public void Save()
		{

			GameObject.FindGameObjectWithTag("GM").GetComponent<UI_popup>().DisableObject(settings, 0.2f);

		}


	}
}