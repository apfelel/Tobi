using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject cardCollection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCardCollection()
    {
        cardCollection.SetActive(!cardCollection.activeInHierarchy);
    }
}
