using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class PopupSlide : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupSlideCanvasGroup;
    [SerializeField] private GameObject popupSlideGameObject;
    [SerializeField] private GameObject popupPanel;


    private Sequence secuenciaAnimacion;
    private bool haEmpezado = false;



    public void Start()
    {
        popupSlideGameObject.SetActive(true);
        popupPanel.SetActive(true);
        popupSlideCanvasGroup.alpha = 1f;
        popupSlideGameObject.transform.localScale = Vector3.zero;
    }

    public void IniciaPopup()
    {   
        Debug.Log("Iniciando Popup");
        popupSlideGameObject.transform.DOScale(1f, 1f)
        .SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            EmpezarAnimacion();
            haEmpezado = true;
        });
    }


    private void EmpezarAnimacion()
    {
        secuenciaAnimacion = DOTween.Sequence()
        .Append(popupSlideCanvasGroup.DOFade(1f, 0.3f))
        .AppendInterval(0.1f)
        .Append(popupSlideCanvasGroup.DOFade(0.4f, 0.3f))
        .AppendInterval(0.1f)
        .SetLoops(-1, LoopType.Yoyo);
    }


    public void TerminarAnimacion()
    {
        if(haEmpezado)
        {
            secuenciaAnimacion?.Kill();

            popupSlideGameObject.transform.DOScale(0f,0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(true);
            });
        }
    }


}
