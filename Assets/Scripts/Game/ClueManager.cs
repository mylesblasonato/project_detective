using EasySurvivalScripts;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public KeyCode openUIKey;
    public GameObject _clueCanvas;
    public Transform _clueParent;
    public Flowchart _notesFlowchart, _addItemEffect;
    public PlayerCamera _playerCamera;
    public NotesManager _nm;

    List<GameObject> _clueList = new List<GameObject>();

    void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _notesFlowchart = GameObject.FindGameObjectWithTag("NotesTween").GetComponent<Flowchart>();
        _clueCanvas = GameObject.FindGameObjectWithTag("ClueCanvas");

        foreach (Transform item in _clueParent)
        {
            _clueList.Add(item.gameObject);
        }
    }

    void Start()
    {
        _clueCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AddClue(GameObject _clue)
    {
        _clue.GetComponent<ClueController>()._clueCollected = true;
        _clue.SetActive(true);
    }

    public void RemoveClue(GameObject _clue)
    {
        _clue.SetActive(false);
    }

    void Update()
    {
        _nm = FindObjectOfType<NotesManager>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _notesFlowchart = GameObject.FindGameObjectWithTag("NotesTween").GetComponent<Flowchart>();

        if (Input.GetKeyDown(openUIKey))
        {
            if (_playerCamera.enabled) _nm.SetIsTalking(false);
            if (!_nm.GetIsTalking())
            {
                _clueCanvas.SetActive(!_clueCanvas.activeSelf);
                _nm.enabled = !_nm.enabled;

                if (_clueCanvas.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    _addItemEffect.SetBooleanVariable("LookingAtClues", true);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    _addItemEffect.SetBooleanVariable("LookingAtClues", false);
                }

                GameManager._instance.SetIsInteracting(!GameManager._instance.GetIsInteracting());
            }
        }
    }
}
