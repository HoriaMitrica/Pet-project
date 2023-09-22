using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHandle : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject nameUI;
    [SerializeField] private string name;
    private Canvas _nameCanvas;
    void Awake()
    {
        nameText.text = name;
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
