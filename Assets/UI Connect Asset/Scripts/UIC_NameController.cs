using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class UIC_NameController : MonoBehaviour
{
    public TextMeshProUGUI _textForName;

    void Update()
    {
        if (_textForName != null)
        {
            GetComponent<UIC_Entity>()._clueName = name;
            _textForName.text = GetComponent<UIC_Entity>()._clueName;           
        }
    }
}
