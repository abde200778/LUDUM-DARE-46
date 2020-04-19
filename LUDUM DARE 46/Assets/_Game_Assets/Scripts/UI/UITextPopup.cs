using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class UITextPopup : MonoBehaviour
{

    private bool m_bInit = false;

    private TextMeshPro tmp;

    

    void Start()
    {
      
    }

    public void Init(string popupTEXT,Color p_color)
    {
        Destroy(this.gameObject, 1.8f);

        tmp = GetComponent<TextMeshPro>();
        tmp.text = popupTEXT;
		tmp.color = p_color;

		transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).OnComplete(ScaleDown);

    
        
        DOTween.PlayAll();
        
        m_bInit = true;
    }

    void Update()
    {
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);

        if (m_bInit)
        {
            float textmeshalpha = tmp.color.a;
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, textmeshalpha -= (0.5f * Time.deltaTime));
        }
    }

    void ScaleDown()
    {
        float maxY = transform.position.y + 3.5f;
        transform.DOMoveY(maxY, 2.0f);

        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1.0f);
    }
    
}
