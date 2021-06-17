﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIC_Entity : MonoBehaviour, I_UIC_Object, I_UIC_Selectable, I_UIC_Draggable, I_UIC_Clickable
{
    public GameObject _examineFlow;
    public string _clueName;

    // v2.0 - local UIC manager
    public UIC_Manager uicManager;

    [Header("Logic")]
    [SerializeField]
    bool _enableDrag;

    public bool EnableDrag { get => _enableDrag; set => _enableDrag = value; }
    public bool enableSelfConnection;

    public bool _enableDelete = true;
    public bool EnableDelete { get => _enableDelete; set => _enableDelete = value; }

    RectTransform _rectTransform;

    [Header("Elements")]
    public List<UIC_Node> nodeList;

    Outline _outline;

    public string ID => name;

    public Vector3[] Handles { get; set; }
    public Color objectColor { get => _image.color; set => _image.color = value; }

    public int Priority => 0;

    public bool DisableClick { get; set; }

    public GameObject _removeButton;

    Image _image;

    void Awake()
    {
        uicManager = GetComponentInParent<UIC_Manager>();

        DisableClick = false;
        _image = GetComponent<Image>();

        // v2.0 - maintain the last parent
        parent = transform.parent;
    }

    void OnValidate()
    {
        Init();

        Awake();
    }

    void Start()
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _clueName;

        Init();
    }

    public void Init()
    {
        _outline = _outline ? _outline : gameObject.GetComponent<Outline>() ? gameObject.GetComponent<Outline>() : gameObject.AddComponent<Outline>();
        if (_outline)
        {
            _outline.effectColor = new Color32(0x7f, 0x5a, 0xf0, 0xFF);
            _outline.effectDistance = new Vector2(3, -3);
            _outline.enabled = false;
        }

        _rectTransform = _rectTransform ? _rectTransform : GetComponent<RectTransform>();

        nodeList = new List<UIC_Node>();
        nodeList.AddRange(transform.GetComponentsInChildren<UIC_Node>());
    }

    public void UpdateConnections()
    {
        foreach (UIC_Node node in nodeList)
        {
            foreach (UIC_Connection conn in node.connectionsList)
            {
                conn.UpdateLine();
            }
        }
    }

    public Image _myImage;
    public GameObject _myImage2, _text;
    bool _isExamining = false;

    public void Select()
    {
        _outline.enabled = true;
        if (!uicManager.selectedUIObjectsList.Contains(this))
        {
            uicManager.selectedUIObjectsList.Add(this);
        }

        _myImage.enabled = false;
        _myImage2.SetActive(false);
        _text.SetActive(false);
        _removeButton.SetActive(false);
    }

    public void Unselect()
    {
        _outline.enabled = false;
        if (uicManager.selectedUIObjectsList.Contains(this))
        {
            uicManager.selectedUIObjectsList.Remove(this);
        }

        _myImage.enabled = true;
        _myImage2.SetActive(true);
        _text.SetActive(true);
        _removeButton.SetActive(true);
    }

    // v2.0 - using not static UIC Manager method
    public void Remove()
    {      
        Unselect();

        foreach (UIC_Node node in nodeList)
        {
            node.RemoveAllConnections();
        }

        if (uicManager.EntityList.Contains(this))
        {
            uicManager.EntityList.Remove(this);
        }

        if (!_enableDelete) return;
        Destroy(gameObject);
    }

    public Transform parent;

    public void OnPointerDown()
    {
        _isExamining = true;

        if (!uicManager.selectedUIObjectsList.Contains(this))
        {
            Select();

            // v2.0 - maintain the last parent
            parent = transform.parent;
        }
        else
        {
            Unselect();
        }
    }

    public void OnPointerUp()
    {
        // v1.2 - using static Manager instance to set parent of dropped entity
        transform.SetParent(uicManager.transform);

        // v2.0 - maintain the last parent
        transform.SetParent(parent);
        
        UpdateConnections();

        if (_isExamining)
            _examineFlow.SetActive(true);
    }

    public void OnDrag()
    {
        _isExamining = false;
        if (EnableDrag)
        {
            Select();
            transform.SetParent(uicManager.pointer.transform.GetChild(0));

            UpdateConnections();
        }
    }

    public List<UIC_Entity> GetConnectedEntities()
    {
        List<UIC_Entity> connectedEntities = new List<UIC_Entity>();
        foreach (UIC_Node node in nodeList)
        {
            foreach (UIC_Connection conn in node.connectionsList)
            {
                UIC_Entity connEntitiy = conn.node0 != node ? conn.node0.entity : conn.node1.entity;
                connectedEntities.Add(connEntitiy);
            }
        }
        return connectedEntities;
    }

    // v1.4 - method to get entities connected to an specific node polarity
    public List<UIC_Entity> GetEntitiesConnectedToPolarity(UIC_Node.PolarityTypeEnum polarity)
    {
        List<UIC_Entity> connectedEntities = new List<UIC_Entity>();
        foreach (UIC_Node node in nodeList)
        {
            if (node.polarityType == polarity)
            {
                foreach (UIC_Connection conn in node.connectionsList)
                {
                    UIC_Entity connEntitiy = conn.node0 != node ? conn.node0.entity : conn.node1.entity;
                    connectedEntities.Add(connEntitiy);
                }
            }
        }
        return connectedEntities;
    }
}
