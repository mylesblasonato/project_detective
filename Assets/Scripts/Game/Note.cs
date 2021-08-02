using UnityEngine;

[CreateAssetMenu]
public class Note : ScriptableObject
{
    public string _noteTextForUI;
    [HideInInspector] public GameObject _uiInstance;

    NotesManager _nm;

    public bool IsCollected { get; set; }

    public bool IsUsed { get; set; } = false;

    public void Load()
    {
        _nm = GameObject.FindObjectOfType<NotesManager>();
        if (IsUsed) _nm.RemoveNote(this);
    }

    public void FindNote()
    {
        _nm = FindObjectOfType<NotesManager>();
        foreach (Transform note in _nm._notesManagerGameObject)
        {
            if(string.Compare(note.gameObject.name, name) == 0)
            {
                note.gameObject.SetActive(true);
            }
        }
    }
}
