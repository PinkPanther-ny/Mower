using UnityEngine;
using UnityEngine.UI;

public class DropdownValueChanged : MonoBehaviour
{
    Dropdown m_Dropdown;
    public GameObject client;
    MowerTCPTestClient c;
    void Start()
    {
        //Fetch the Dropdown GameObject
        m_Dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChange(m_Dropdown);
        });

        c = client.GetComponent<MowerTCPTestClient>();
    }

    void DropdownValueChange(Dropdown change)
    {
        c.SendAction("EXIT");
        c.SendAction(change.options[change.value].text.ToUpper());
    }
}