using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueController : MonoBehaviour
{
    public UIC_Entity _linkedClue;
    public Note _noteToReward;
    public Transform _content;

    UIC_Entity _clue01;
    NotesManager _nm;

    void Awake()
    {
        _nm = FindObjectOfType<NotesManager>();
        _clue01 = GetComponent<UIC_Entity>();
    }

    void Update()
    {
        var linkedClues = _clue01.GetConnectedEntities();
        if (linkedClues.Contains(_linkedClue))
        {
            if (_nm._noteInventory.Contains(_noteToReward)) return;
            _nm.AddNote(_noteToReward);

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
