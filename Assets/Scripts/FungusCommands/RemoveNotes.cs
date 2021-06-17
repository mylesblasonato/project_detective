using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("MadDog", "Remove Notes", "Remove multiple notes")]
public class RemoveNotes : Command
{
    [SerializeField] List<Note> _notesToRemove;
    [SerializeField] float _waitDuration;

    NotesManager _nm;

    private void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
    }

    public override void OnEnter()
    {
        Execute();
    }

    public override void Execute()
    {
        StartCoroutine("RemoveNotesTimer");
    }

    IEnumerator RemoveNotesTimer()
    {
        foreach (var item in _notesToRemove)
        {
            _nm.RemoveNote(item);
            yield return new WaitForSeconds(_waitDuration);           
        }
        Continue();
    }
}
