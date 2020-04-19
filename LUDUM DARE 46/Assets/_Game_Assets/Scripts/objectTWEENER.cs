using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class objectTWEENER : MonoBehaviour
{


	[SerializeField]private Tween m_tween;
	[SerializeField]private Ease scaleEaseType;
	[SerializeField]private Ease rotationEaseType;
	[SerializeField]private Vector3  targetscale;
	[SerializeField]private float scaleDuration;
	[SerializeField]private float esr;
	[SerializeField]private int vebrato;
	[SerializeField]private Vector3  targetRotation;
	[SerializeField]private float rotationDuration;
	[SerializeField]private RotateMode rotateMode;
	[SerializeField] private bool Islocal;
	[SerializeField] private bool noRotation;


    // Start is called before the first frame update
    void Start()
    {
		transform.DOPunchScale(targetscale, scaleDuration, vebrato, esr).SetEase(scaleEaseType).SetLoops(-1);
		if (!noRotation)
		{
			if (Islocal)
				transform.DOLocalRotate(targetRotation, rotationDuration, rotateMode).SetEase(rotationEaseType).SetLoops(-1);
			else
				transform.DORotate(targetRotation, rotationDuration, rotateMode).SetEase(rotationEaseType).SetLoops(-1);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
