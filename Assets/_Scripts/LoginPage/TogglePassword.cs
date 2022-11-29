using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglePassword : MonoBehaviour
{
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button btn;
    
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(Toggle);
    }

    void Toggle()
    {
        password.contentType = (password.contentType == TMP_InputField.ContentType.Password) ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        password.ForceLabelUpdate();
    }
}
