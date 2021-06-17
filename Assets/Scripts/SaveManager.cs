using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Types;
using BayatGames.SaveGameFree.Serializers;

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

    [Header("SAVE DATA")]
    public Transform _player;
    public Transform _cluesParent;

    public void Save()
    {
        // SAVE PLAYER
        var playerTransformData = new PlayerTransformData(_player.position, _player.eulerAngles);
        SaveGame.Save<PlayerTransformData>("PlayerData", playerTransformData, new SaveGameBinarySerializer());

        // SAVE CLUES
        foreach (Transform clue in _cluesParent)
        {
            SaveGame.Save<bool>(clue.name, clue.gameObject.activeSelf, new SaveGameBinarySerializer());
        }
    }

    public void Load()
    {
        // LOAD PLAYER
        var playerTransformData = SaveGame.Load<PlayerTransformData>("PlayerData", new PlayerTransformData(Vector3.zero, Vector3.zero), new SaveGameBinarySerializer());
        _player.SetPositionAndRotation(playerTransformData.pos, Quaternion.Euler(playerTransformData.rot));

        // LOAD CLUES
        foreach (Transform clue in _cluesParent)
        {
            var hasClue = SaveGame.Load<bool>(clue.name, false, new SaveGameBinarySerializer());
            clue.gameObject.SetActive(hasClue);
        }
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
