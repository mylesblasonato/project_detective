using EasySurvivalScripts;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueController : MonoBehaviour
{
    public UIC_Entity _linkedClue;
    public List<Note> _notesToReward;
    public Transform _content;

    UIC_Entity _clue01;
    NotesManager _nm;
    PlayerCamera _playerCamera;

    void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
        _clue01 = GetComponent<UIC_Entity>();
        _playerCamera = FindObjectOfType<PlayerCamera>();
    }

    void Update()
    {
        var linkedClues = _clue01.GetConnectedEntities();
        if (linkedClues.Contains(_linkedClue))
        {
            if (!_nm.GetIsTalking())
            {
                foreach (var note in _notesToReward)
                {
                    if (_nm._noteInventory.Contains(note))
                        return;
                }

                _nm.OpenNotes();
                var addNotesCommand = gameObject.AddComponent<AddNotes>();
                addNotesCommand.Initialize(1f, _notesToReward, true);
                addNotesCommand.Execute();

                foreach (Transform note in _content)
                {
                    var btn = note.GetComponent<Button>();
                    if (btn != null)
                        note.GetComponent<Button>().interactable = false;
                }

                Destroy(this);
            }
        }
    }
}
