using EasySurvivalScripts;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
    [HideInInspector] public Transform _notesManagerGameObject;
    public Flowchart _examineNotesFlow;
    public Transform _viewportContentForNotes;
    public GameObject _noteButtonPrefab;
    public GameObject _cursor;
    public GameObject _addItemEffect;
    public TextMeshProUGUI _activeNoteText;
    public Note _activeNote;
    public HashSet<Note> _noteInventory = new HashSet<Note>();
    public GameObject _activeInteractor;
    public Flowchart _notesFlowTween;
    public KeyCode _notesUI;
    public ClueManager _cm;
    public string _openNotesMessage, _closeNotesMessage;

    bool _isTalking = false;
    PlayerCamera _playerCamera;
    PlayerMovement _playerMovement;

    void Awake()
    {
        _notesManagerGameObject = transform;
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        _cm = FindObjectOfType<ClueManager>();
        _cursor = GameObject.FindGameObjectWithTag("Cursor");
        if (_activeNote != null) AddNote(_activeNote);
    }

    public void SetIsTalking(bool isTalking) => _isTalking = isTalking;
    public bool GetIsTalking() => _isTalking;

    bool _isUiOpen = false;
    void Update()
    {
        if(Input.GetKeyDown(_notesUI))
        {
            if (_playerCamera.enabled) SetIsTalking(false);
            if (!_isTalking)
            {
                if (_notesFlowTween.GetBooleanVariable("IsUp"))
                {
                    _notesFlowTween.SendFungusMessage("Close");
                    _cm.enabled = true;
                    GameManager._instance.SetIsInteracting(false);
                    _cursor.SetActive(false);
                }
                else
                {
                    foreach (Transform note in _viewportContentForNotes)
                    {
                        var btn = note.GetComponent<Button>();
                        if (btn != null && !btn.transform.GetChild(1).gameObject.activeSelf)
                            note.GetComponent<Button>().interactable = true;
                    }
                    _notesFlowTween.SendFungusMessage("Open");
                    _cm.enabled = false;
                    GameManager._instance.SetIsInteracting(true);
                    _cursor.SetActive(true);
                }
            }
        }
    }

    public void SetActiveNote(Note note)
    {
        if(_noteInventory.Contains(note))
        {
            _activeNote = note;
            _activeNoteText.text = note._noteTextForUI;
            _playerMovement.GetComponent<InteractionController>().Interact();
            _notesFlowTween.SendFungusMessage("Close");
        }
    }

    public void AddNote(Note note)
    {
        _noteInventory.Add(note);
        var go = Instantiate(_noteButtonPrefab, _viewportContentForNotes);
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note._noteTextForUI;

        foreach (Transform nt in _viewportContentForNotes)
        {
            var btn = nt.GetComponent<Button>();
            if (btn != null)
                nt.GetComponent<Button>().interactable = false;
        }

        go.GetComponent<Button>().onClick.AddListener(() => SetActiveNote(note));
        go.transform.GetChild(2).gameObject.SetActive(true);
        note._uiInstance = go;
    }

    public void RemoveNote(Note note)
    {
        note.IsUsed = true;
        note._uiInstance.transform.GetChild(1).gameObject.SetActive(true);
        note._uiInstance.GetComponent<Button>().interactable = false;
        note._uiInstance.GetComponent<Button>().onClick.RemoveListener(() => SetActiveNote(note));
    }

    public void OpenNotes()
    {
        _addItemEffect.GetComponent<Flowchart>().SendFungusMessage(_openNotesMessage);
    }

    public void CloseNotes()
    {
        _addItemEffect.GetComponent<Flowchart>().SendFungusMessage(_closeNotesMessage);
    }
}
