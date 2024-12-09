using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, IPointerEnterHandler
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
        _selectable.Select();
    }
}
