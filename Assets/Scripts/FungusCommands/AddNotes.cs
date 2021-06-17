using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("MadDog", "Add Notes", "Adds multiple notes")]
public class AddNotes : Command
{
    [SerializeField] List<Note> _notesToAdd;
    [SerializeField] float _waitDuration;

    bool _isSenderAclue = false;
    NotesManager _nm;

    private void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
    }

    public void Initialize(float waitDuration, List<Note> notesToAdd, bool isSenderAclue = false)
    {
        _waitDuration = waitDuration;
        _notesToAdd = notesToAdd;
        _isSenderAclue = isSenderAclue;
    }

    public override void OnEnter()
    {
        Execute();       
    }

    public override void Execute()
    {
        _nm.SetIsTalking(true);
        StartCoroutine("AddNotesTimer");
    }

    IEnumerator AddNotesTimer()
    {
        foreach (var item in _notesToAdd)
        {
            _nm.AddNote(item);
            yield return new WaitForSeconds(_waitDuration);
        }

        yield return new WaitForSeconds(_waitDuration);
        if (_isSenderAclue)
        {          
            _nm.CloseNotes();
            _nm.SetIsTalking(false);
        }

        Continue();
    }
}
