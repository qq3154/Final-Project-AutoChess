using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProfile : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField fullname;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private Button btn;
    
    private void Start()
    {
        btn.onClick.AddListener(OnUpdateProfile);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void SetData()
    {
        username.text = UserManager.instance.username;
        fullname.text = UserManager.instance.fullName;
        email.text = UserManager.instance.email;
    }

    public void OnUpdateProfile()
    {
        string usernameStr = username.text;
        string fullnameStr = fullname.text;
        string emailStr = email.text;
        ApiRequest.instance.SendUpdateProfileRequest(usernameStr, fullnameStr, emailStr);
    }
}
