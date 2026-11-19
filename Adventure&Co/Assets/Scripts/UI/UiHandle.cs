using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UiHandle : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject nameUI;
    // Renamed from 'name' (it hid Object.name). FormerlySerializedAs keeps existing scene values.
    [FormerlySerializedAs("name")]
    [SerializeField] private string displayName;
    private Canvas _nameCanvas;
    void Awake()
    {
        nameText.text = displayName;
        _nameCanvas=nameUI.GetComponent<Canvas>();
    }

    public void CanvasEnable()
    {
        _nameCanvas.enabled = true;
    }
    public void CanvasDisable()
    {
        _nameCanvas.enabled = false;
    }
}
