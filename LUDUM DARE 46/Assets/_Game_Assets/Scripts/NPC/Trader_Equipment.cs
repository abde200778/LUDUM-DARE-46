using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Huntrox.Games.LD46
{
	[System.Serializable]
	public class Upgrade
	{
		public string name;
		[TextArea(5, 10)]
		public string Description;
		public upgrades _upgrade;
		public int price;
		public int index;

	}
	public class Trader_Equipment : MonoBehaviour
	{

		private GameObject GM;

		public static Trader_Equipment inctanse;
		[SerializeField]private List<Upgrade> _upgrades = new List<Upgrade>();

		private GameObject Trader_equipment_ui;

		void Awake() => inctanse = this;


		private void Start()
		{
			GM = GameObject.FindGameObjectWithTag("GM");

			Trader_equipment_ui = GameObject.Find("Canvas").transform.Find("Trader_equipment_ui").gameObject;

		}
		public void OpenTrader()
		{
			GM.GetComponent<UI_popup>().EnableObject(Trader_equipment_ui, .3f,true);


		}
		public void CloseTrader()
		{
			GM.GetComponent<UI_popup>().DisableObject(Trader_equipment_ui, .2f);


		}









	}
	public enum upgrades {Boat_T2, Fishing_Rod_T2, Fishing_Rod_T3, Bucket_T_1, Bucket_T_2, Bucket_T_3 ,Fishing_Lure_T1, Fishing_Lure_T2, Fishing_Lure_T3,Fishing_Gear_1, Fishing_Gear_2}
}