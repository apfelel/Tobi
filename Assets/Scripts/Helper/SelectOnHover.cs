using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Selectable _selectable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.gameObject == this)
        {
            UIManager.Instance.PlayHover();
        }
        _selectable.Select();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.PlayClick();
    }
}
