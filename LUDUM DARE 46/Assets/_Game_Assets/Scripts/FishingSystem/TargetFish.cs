using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;


namespace Huntrox.Games.LD46
{

	public class TargetFish : MonoBehaviour
	{

		[SerializeField]private GameObject[] diractionArrows;
		[SerializeField]private GameObject textpopup;
		[SerializeField] private Color defaultColor;
		[SerializeField] private Color hitColor;
		[SerializeField] private Color misColor;
		public  FishBuff buff;
		private Vector3 target;
		private float speed=2f;
		private NavMeshAgent agent;

		Tween m_tween;

		void Awake()
		{
			FishingSystem.instance.onBuffChanged += SetBuff;
			//agent = GetComponent<NavMeshAgent>();
			//	diractionArrows = new GameObject[] {
			//       transform.Find("ArrowUp").gameObject,
			//       transform.Find("ArrowDown").gameObject,
			//       transform.Find("ArrowLeft").gameObject,
			//       transform.Find("ArrowRight").gameObject, };
			m_tween = diractionArrows[0].GetComponent<MeshRenderer>().material.DOColor(defaultColor, "_BaseColor",0.01f);
		}
		private void Start()
		{
			transform.localPosition = new Vector3(transform.localPosition.x, 2.8f, transform.localPosition.z);
		}

		public void initialize()
		{

		}

		public void SetBuff(FishBuff p_buff)
		{



			StartCoroutine(SetupArrows(p_buff));
		
		}
		public void unsubscribe()
		{

			
		}
		private IEnumerator SetupArrows(FishBuff p_buff)
		{

			yield return m_tween.WaitForCompletion();

			for (int i = 0; i < diractionArrows.Length; i++)
			{
				if (i == (int)p_buff)
				{
					diractionArrows[i].transform.localScale = new Vector3(0.16846f, 0.16846f, 0.16846f);
					diractionArrows[i].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", defaultColor);
					diractionArrows[i].SetActive(true);

				}
				else
				{
					diractionArrows[i].SetActive(false);
				}
			}
		}

		public void shake(bool value)
		{

			Color t_color;
			GameObject t_popup = Instantiate(textpopup, transform.position + new Vector3(0,.5f,0), Quaternion.identity);

			if (value)
			{
				t_color = hitColor;
				AudioManager.inctanse.PlayOneshot(SFX_TYPE.Hit_SFX,0);
				t_popup.GetComponent<UITextPopup>().Init("HIT", t_color);
			}
			else
			{
				t_color = misColor;
				AudioManager.inctanse.PlayOneshot(SFX_TYPE.Miss_SFX,0);
				t_popup.GetComponent<UITextPopup>().Init("Miss", t_color);
			}
			foreach (GameObject t_obgect in diractionArrows)
			{


				t_obgect.GetComponent<MeshRenderer>().material.DOColor(t_color, "_BaseColor", .2f);

				//t_obgect.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", misColor);


				m_tween = t_obgect.transform.DOShakeScale(0.2f, new Vector3(.25f, 0, .25f), 15, 90,true).OnComplete(reset); 
			}



		}
	       void reset()
		{

			foreach (GameObject t_obgect in diractionArrows)
			{
				t_obgect.SetActive(false);
				t_obgect.transform.localScale = new Vector3(0.16846f, 0.16846f, 0.16846f);

				//t_obgect.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", defaultColor);
			}


			
		}
		public void SetDirection(Vector3 p_dirc)
		{
	

			p_dirc.y = 2.87f;
			//transform.rotation = Quaternion.LookRotation(target);
			

			target = p_dirc;

			
		}
		// Update is called once per frame
		void Update()
		{

			if(target != null)
			{
				//Vector3 direction = (target - transform.position).normalized;
				//agent.SetDestination(direction);
				Quaternion lookRotation = Quaternion.LookRotation(new Vector3(target.x, 0, target.z));
				transform.Find("FishSkin").rotation = Quaternion.Slerp(transform.Find("FishSkin").rotation, lookRotation, Time.deltaTime * 8f);
				//transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);

				//transform.Find("FishSkin").rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right) * Quaternion.LookRotation(target);
				//	transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right) * Quaternion.LookRotation(target);
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

			}




		}

		private void OnDestroy()
		{
			FishingSystem.instance.onBuffChanged -= SetBuff;
		}
		private void OnDrawGizmos()
		{
			//Gizmos.color = Color.red;
//Gizmos.DrawSphere(target, 2.5f);
		}

	}//public enum FishBuff { Up, Down, Left, Right }
}