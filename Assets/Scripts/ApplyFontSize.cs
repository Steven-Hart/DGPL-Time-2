using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ApplyFontSize : MonoBehaviour {

    private TextMeshProUGUI roleTextUI;
    private Text[] namesTextUI;
    private int roleTextFontSize;
    private int nameTextFontSize;
	
	void Start() {
		roleTextUI = GetComponentInChildren<TextMeshProUGUI>();
		namesTextUI = GetComponentsInChildren<Text>();
		roleTextFontSize = GetComponentInParent<SetFontSizes>().roleFontSize;
		nameTextFontSize = GetComponentInParent<SetFontSizes>().nameFontSize;
		ResizeFont();
	}
	void Update () {
    }

    //private void Update()
    //{
    //    ResizeFont();
    //}

    void ResizeFont()
    {
        roleTextUI.fontSize = roleTextFontSize;
        foreach (Text t in namesTextUI)
        {
            t.fontSize = nameTextFontSize;
        }
    }
}
