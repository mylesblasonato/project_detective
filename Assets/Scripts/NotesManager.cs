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

    PlayerMovement _playerMovement;

    void Awake()
    {
        _notesManagerGameObject = transform;
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _cm = FindObjectOfType<ClueManager>();
        _cursor = GameObject.FindGameObjectWithTag("Cursor");
        if (_activeNote != null) AddNote(_activeNote);
    }

    bool _isUiOpen = false;
    void Update()
    {
        if(Input.GetKeyDown(_notesUI))
        {
            if (_notesFlowTween.GetBooleanVariable("IsUp"))
            {
                _notesFlowTween.SendFungusMessage("Close");
                GameManager._instance.SetIsInteracting(false);
                _cursor.SetActive(false);
            }
            else
            {
                foreach (Transform note in _viewportContentForNotes)
                {
                    var btn = note.GetComponent<Button>();
                    if (btn != null)
                        note.GetComponent<Button>().interactable = true;
                }

                _notesFlowTween.SendFungusMessage("Open");
                GameManager._instance.SetIsInteracting(true);
                _cursor.SetActive(true);
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
        _addItemEffect.SetActive(true);
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
        note._uiInstance = go;
    }

    public void RemoveNote(Note note)
    {
        _addItemEffect.SetActive(true);
        _noteInventory.Remove(note);     
        note._uiInstance.GetComponent<Button>().onClick.RemoveListener(() => SetActiveNote(note));
        Destroy(note._uiInstance);
    }
}
