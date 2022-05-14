using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeEnhanceViewer : MonoBehaviour
{
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;

    private Outline selectOutline;

    private Enhance upgradeEnhance;

    private void Awake()
    {
        selectOutline = GetComponent<Outline>();
        selectOutline.enabled = false;
    }

    private void OnMouseEnter()
    {
        selectOutline.enabled = true;
    }

    private void OnMouseExit()
    {
        selectOutline.enabled = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
    }

    public void DrawEnhance(Enhance enhance)
    {
        thumbnail.sprite = enhance.Icon;
        name.text = enhance.name;
        description.text = enhance.Description;
        upgradeEnhance = enhance;
    }
}
