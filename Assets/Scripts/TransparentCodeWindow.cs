﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransparentCodeWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float fadeSpeed = 1.0f;

    private float targetAlpha = 1.0f;
    private Image codeWindow;
    private Image codeWindowMenu;

    private void Awake()
    {
        codeWindow = CodeWindow.Instance.GetComponent<Image>();
        codeWindowMenu = CodeWindow.Instance.Menu.Background;
    }

    private void OnEnable()
    {
        targetAlpha = 1.0f;
        codeWindow.color = new Color(codeWindow.color.r, codeWindow.color.g, codeWindow.color.b, targetAlpha);
        codeWindowMenu.color = new Color(codeWindowMenu.color.r, codeWindowMenu.color.g, codeWindowMenu.color.b, targetAlpha);
    }

    private void Update()
    {
        codeWindow.color = new Color(codeWindow.color.r, codeWindow.color.g, codeWindow.color.b, Mathf.MoveTowards(codeWindow.color.a, targetAlpha, fadeSpeed * Time.deltaTime));
        codeWindowMenu.color = new Color(codeWindowMenu.color.r, codeWindowMenu.color.g, codeWindowMenu.color.b, Mathf.MoveTowards(codeWindowMenu.color.a, targetAlpha, fadeSpeed * Time.deltaTime));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAlpha = 0.4f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAlpha = 1.0f;
    }
}
