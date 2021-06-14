using UnityEngine;

[CreateAssetMenu]
public class Note : ScriptableObject
{
    public string _noteNameInNotesManager;
    public string _noteTextForUI;
    public string _examineMessage;
    [HideInInspector] public GameObject _uiInstance;

    NotesManager _nm;

    public void FindNote()
    {
        _nm = FindObjectOfType<NotesManager>();
        foreach (Transform note in _nm._notesManagerGameObject)
        {
            if(string.Compare(note.gameObject.name, _noteNameInNotesManager) == 0)
            {
                note.gameObject.SetActive(true);
            }
        }
    }
}