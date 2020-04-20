using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Huntrox.Games.LD46
{
	public class UI_popup : MonoBehaviour
	{
		[SerializeField] private Ease ease;
		[SerializeField] private float moveTimer = .15f;
		[SerializeField] private int curve = 10;
		[SerializeField] private Tween _tween;
		[SerializeField] private Tween scaleTween;
		[SerializeField] private GameObject Target;
		[SerializeField] private GameObject UITextPopup;
		[SerializeField] private Transform CashPopupPos;
		//[SerializeField] private bool IsActive;
		[SerializeField] private float esr;
		[SerializeField] private int vebrato;
		[SerializeField] private float Textesr;
		[SerializeField] private int Textvebrato;
		[SerializeField] private Ease scaleEaseType;
		[SerializeField] private Ease TextEase;
		// Start is called before the first frame update
		void Start()
		{
			//Target = transform.Find("F").gameObject;

		}



		public void EnableObject(GameObject targetObject, float p_duration, bool popup_sfx)
		{


			StartCoroutine(SetActive(targetObject, p_duration, popup_sfx));

		}

		public void DisableObject(GameObject targetObject, float p_duration)
		{
			StartCoroutine(DisActive(targetObject, p_duration));
			if (targetObject.name == "target_Fish_UI") setTween_null();

		}



		public void PunchUi(GameObject targetObject, float p_duration, Vector3 targetscale)
		{
			if (scaleTween == null)
			{
				scaleTween = targetObject.transform.DOPunchScale(targetscale, p_duration, vebrato, esr).SetEase(scaleEaseType).SetLoops(-1);
				targetObject.GetComponent<TextMeshProUGUI>().DOColor(Color.red, p_duration);
			}
		}



		void setTween_null()
		{
			scaleTween = null;

		}


		public void AddCashText(GameObject targetObject, float p_duration, Vector3 targetscale, Color p_colorcolor, string text)
		{
			targetObject.transform.localScale = new Vector3(1f, 1f, 1f);
			StartCoroutine(addCashText(targetObject, p_duration, targetscale, p_colorcolor, text));

		}

		IEnumerator addCashText(GameObject targetObject, float p_duration, Vector3 targetscale, Color p_colorcolor, string text)
		{

			GameObject t_go = Instantiate(UITextPopup, CashPopupPos.position + new Vector3(0.5f, 0, 0), Quaternion.identity, CashPopupPos);
			t_go.GetComponent<Ui_TextpopupGUI>().Init(text, p_colorcolor);
			targetObject.transform.DOPunchScale(targetscale, p_duration, Textvebrato, Textesr).SetEase(TextEase);
			Tween t_tween = targetObject.GetComponent<TextMeshProUGUI>().DOColor(Color.grey, p_duration);
			yield return t_tween.WaitForCompletion();
			targetObject.GetComponent<TextMeshProUGUI>().DOColor(Color.white, p_duration);

		}

		IEnumerator DisActive(GameObject targetObject, float p_duration)
		{

			if (targetObject.activeSelf)
			{
				if (_tween != null)
				{
					yield return _tween.WaitForCompletion();
				}
				_tween = targetObject.transform.DOScale(0f, p_duration).SetEase(ease);
				yield return _tween.WaitForCompletion();
				targetObject.SetActive(false);



				yield break;
			}

		}
		IEnumerator SetActive(GameObject targetObject, float p_duration, bool popup_sfx)
		{
			if (!targetObject.activeSelf)
			{

				if (_tween != null)
					yield return _tween.WaitForCompletion();

				targetObject.SetActive(true);

				if (popup_sfx)
					AudioManager.inctanse.PlayOneshot(SFX_TYPE.Popup_SFX, 0f);

				_tween = targetObject.transform.DOScale(1f, p_duration).SetEase(ease);


				yield break;
			}
		}
		




	}
}