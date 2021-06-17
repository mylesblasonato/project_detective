using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("MadDog", "Close Notes", "Close notes ui")]
public class CloseNotes : Command
{
    NotesManager _nm;

    private void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
    }

    public override void OnEnter()
    {
        _nm.CloseNotes();
        Continue();
    }
}
