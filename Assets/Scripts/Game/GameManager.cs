using EasySurvivalScripts;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _cursor;
    public GameObject _clueCanvas;
    public GameObject _saveCanvas;
    public Flowchart _notesCanvas;
    public GameObject[] _pointerCursors;

    bool _isInteracting = false;
    PlayerMovement _pm;
    PlayerCamera _pc;
    InteractionController _ic;
    ExamineController _ec;

    public static GameManager _instance;

    void Awake()
    {
        _instance = this;

        _pm = FindObjectOfType<PlayerMovement>();
        _pc = FindObjectOfType<PlayerCamera>();
        _ic = _pm.gameObject.GetComponent<InteractionController>();
        _ec = _pm.gameObject.GetComponent<ExamineController>();
        _clueCanvas = GameObject.FindGameObjectWithTag("ClueCanvas");
    }

    void OnEnable()
    {
        _instance = this;

        _pm = FindObjectOfType<PlayerMovement>();
        _pc = FindObjectOfType<PlayerCamera>();
        _ic = _pm.gameObject.GetComponent<InteractionController>();
        _ec = _pm.gameObject.GetComponent<ExamineController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _cursor.transform.position = Input.mousePosition;
        Cursor.visible = false;

        if (_isInteracting)
        {
            CanInteract(false);
        }
        else
        {
            CanInteract(true);
        }
    }

    public void ShowPointerCursors(bool show)
    {
        foreach (var item in _pointerCursors)
        {
            item.SetActive(show);
        }
    }

    public void SetIsInteracting(bool val) { _isInteracting = val; }
    public bool GetIsInteracting() => _isInteracting;

    void CanInteract(bool isInteracting)
    {
        if (_pm == null) return;
        _pm.enabled = isInteracting;
        _pc.enabled = isInteracting;
        _ic.enabled = isInteracting;
        _ec.enabled = isInteracting;

        if (!isInteracting)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
