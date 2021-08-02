using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Types;
using BayatGames.SaveGameFree.Serializers;
using Fungus;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerTransformData
{
    public Vector3Save pos;
    public Vector3Save rot;

    public PlayerTransformData(Vector3Save p, Vector3Save r)
    {
        pos = p;
        rot = r;
    }
}

public class SaveManager : MonoBehaviour
{
    public KeyCode _pauseKey;
    public GameObject _ui;
    public GameObject _cursor;
    public NotesManager _nm;
    public ClueManager _cm;
    public SaveMenu _sm;

    [Header("SAVE DATA")]
    public Transform _player;
    public Transform _cluesParent;
    public Transform _interactables;

    public void DeleteSave()
    {
        SaveGame.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Save();
    }

    public void Save()
    {
        SaveGame.Clear();

        // SAVE PLAYER
        var playerTransformData = new PlayerTransformData(_player.position, _player.eulerAngles);
        SaveGame.Save<PlayerTransformData>("PlayerData", playerTransformData, new SaveGameBinarySerializer());

        // SAVE INTERACTABLES
        foreach (Transform item in _interactables)
        {
            var interactableTransformData = new PlayerTransformData(item.position, item.eulerAngles);
            SaveGame.Save<PlayerTransformData>("InteractableData" + item.name, interactableTransformData, new SaveGameBinarySerializer());
            SaveGame.Save<bool>("InteractableData" + item.name + "active", item.gameObject.activeSelf, new SaveGameBinarySerializer());
            SaveGame.Save<bool>("InteractableData" + item.name + "clueCollected", item.GetChild(0).GetComponent<Flowchart>().GetBooleanVariable("ClueCollected"), new SaveGameBinarySerializer());
            SaveGame.Save<bool>("InteractableData" + item.name + "noteUsed", item.GetChild(0).GetComponent<Flowchart>().GetBooleanVariable("NoteUsed"), new SaveGameBinarySerializer());
        }      

        // SAVE CLUES
        foreach (Transform clue in _cluesParent)
        {
            SaveGame.Save<bool>(clue.name, clue.GetComponent<ClueController>()._clueCollected, new SaveGameBinarySerializer());
            SaveGame.Save<bool>(clue.name + "enabled", clue.GetComponent<ClueController>().enabled, new SaveGameBinarySerializer());
            SaveGame.Save<bool>(clue.name + "linked", clue.GetComponent<ClueController>()._clueLinked, new SaveGameBinarySerializer());
        }

        int noteIndex = 0;
        // SAVE NOTES
        foreach (Note note in _nm._noteInventory)
        {
            SaveGame.Save<string>("note " + noteIndex, note.name, new SaveGameBinarySerializer());
            SaveGame.Save<bool>(note.name + noteIndex + "collected", note.IsCollected, new SaveGameBinarySerializer());
            SaveGame.Save<bool>(note.name + noteIndex + "used", note.IsUsed, new SaveGameBinarySerializer());

            noteIndex++;
        }
        SaveGame.Save<int>("noteCount", _nm._noteInventory.Count, new SaveGameBinarySerializer());

        _sm.Save();
    }

    public void Load()
    {
        // LOAD PLAYER
        var playerTransformData = SaveGame.Load<PlayerTransformData>("PlayerData", new PlayerTransformData(Vector3.zero, Vector3.zero), new SaveGameBinarySerializer());
        _player.SetPositionAndRotation(playerTransformData.pos, Quaternion.Euler(playerTransformData.rot));

        // SAVE INTERACTABLES
        foreach (Transform item in _interactables)
        {
            var interactableTransformData = SaveGame.Load<PlayerTransformData>("InteractableData" + item.name, new PlayerTransformData(Vector3.zero, Vector3.zero), new SaveGameBinarySerializer());
            item.SetPositionAndRotation(interactableTransformData.pos, Quaternion.Euler(interactableTransformData.rot));
            item.gameObject.SetActive(SaveGame.Load<bool>("InteractableData" + item.name + "active", item.gameObject.activeSelf, new SaveGameBinarySerializer()));
            item.GetChild(0).GetComponent<Flowchart>().SetBooleanVariable("ClueCollected", SaveGame.Load<bool>("InteractableData" + item.name + "clueCollected", false, new SaveGameBinarySerializer()));
            item.GetChild(0).GetComponent<Flowchart>().SetBooleanVariable("NoteUsed", SaveGame.Load<bool>("InteractableData" + item.name + "noteUsed", false, new SaveGameBinarySerializer()));
        }

        // LOAD CLUES
        foreach (Transform clue in _cluesParent)
        {
            var hasClue = SaveGame.Load<bool>(clue.name, false, new SaveGameBinarySerializer()); 
            clue.GetComponent<ClueController>()._clueCollected = hasClue;
            clue.gameObject.SetActive(clue.GetComponent<ClueController>()._clueCollected);
            clue.GetComponent<ClueController>().enabled = SaveGame.Load<bool>(clue.name + "enabled", false, new SaveGameBinarySerializer());
            clue.GetComponent<ClueController>()._clueLinked = SaveGame.Load<bool>(clue.name + "linked", false, new SaveGameBinarySerializer());
            var inOut = clue.GetChild(1).GetChild(0);
            inOut.GetComponent<UIC_Node>().ConnectTo(clue.GetComponent<ClueController>()._linkedNode);
        }

        int noteIndex = 0;
        // LOAD NOTES
        foreach (Transform note in _nm._viewportContentForNotes)
        {
            Destroy(note.gameObject);
        }
        while (noteIndex < SaveGame.Load<int>("noteCount", _nm._noteInventory.Count, new SaveGameBinarySerializer()))
        {
            string nam = SaveGame.Load<string>("note " + noteIndex, "Unknown Clue", new SaveGameBinarySerializer());
            Note note = Resources.Load<Note>("GameData/Notes/" + nam);
            note.IsCollected = SaveGame.Load<bool>(note.name + noteIndex + "collected", false, new SaveGameBinarySerializer());           
            _nm.AddNote(note);
            note.IsUsed = SaveGame.Load<bool>(note.name + noteIndex + "used", false, new SaveGameBinarySerializer());
            note.Load();

            noteIndex++;
        }

        _sm.Load();
    }

    void Update()
    {
        if(Input.GetKeyDown(_pauseKey))
        {
            GameManager._instance.SetIsInteracting(!GameManager._instance.GetIsInteracting());
            _nm.enabled = !_nm.enabled;
            _player.GetComponent<InteractionController>().enabled = !_player.GetComponent<InteractionController>().enabled;
            _cm.enabled = !_cm.enabled;      
            _ui.SetActive(!_ui.activeSelf);

            if (_ui.activeSelf)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
