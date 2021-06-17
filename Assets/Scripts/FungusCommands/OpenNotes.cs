using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("MadDog", "Open Notes", "Open notes ui")]
public class OpenNotes : Command
{
    NotesManager _nm;

    private void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
    }

    public override void OnEnter()
    {
        _nm.OpenNotes();
        Continue();
    }
}
