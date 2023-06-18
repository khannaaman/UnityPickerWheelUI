using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UIElements;
using System.Drawing;
using DG.Tweening;

public class Demo : MonoBehaviour {

   [SerializeField] private UnityEngine.UI.Button uiSpinButton ;

   [SerializeField] private Text uiSpinButtonText ;

   [SerializeField] private PickerWheel[] pickerWheels;

   [SerializeField] private UnityEngine.UI.Toggle[] toggles;

   [SerializeField] private TMP_Text toggleSelectionError;

   [SerializeField] private GameObject confettiPrefab;

   [SerializeField] private UnityEngine.UI.Toggle removeCelebrations;

    private void Start()
    {
        AddListeners();
        SetGameObjectActiveState();
    }

    private void AddListeners()
    {
        foreach (var item in toggles)
        {
            item.onValueChanged.AddListener(a => SetGameObjectActiveState());
        }

        uiSpinButton.onClick.AddListener(() => {

            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "Spinning";

            foreach (var val in toggles)
            {
                val.enabled = false;
            }

            foreach (var item in pickerWheels)
            {
                item.OnSpinEnd(wheelPiece => {
                    Debug.Log(
                       @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                       + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                    );

                    foreach (var val in toggles)
                    {
                        val.enabled = true;
                        if(val.GetComponentInChildren<Text>().text == wheelPiece.Label)
                        {
                            UnityEngine.Color color = new UnityEngine.Color(0.98f, 0.50f, 0.16f);
                            val.GetComponentInChildren<TextMeshProUGUI>().color = color;
                            item.WheelPieceTransforms[wheelPiece.Index].GetChild(1).GetComponent<Text>().color = color;
                        }
                    }

                    TriggerCelebrations();

                    uiSpinButton.interactable = true;
                    uiSpinButtonText.text = "Spin";
                });
            }

            pickerWheels.Single(pw => pw.gameObject.activeInHierarchy).Spin();
        });
    }


    private void SetGameObjectActiveState()
    {
        //populate sorted list
        List<string> labels = GetToggleLabels();
        int count = labels.Count();

        foreach (var item in pickerWheels)
        {
            if (item.wheelPieces.Length == count)
            {
                item.gameObject.SetActive(true);
                SetWheelPieceLabel(labels, item);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }

        toggleSelectionError.gameObject.SetActive(count < 1);
        uiSpinButton.gameObject.SetActive(count > 0);
    }

    private List<string> GetToggleLabels()
    {
        List<string> lst = new List<string>();
        //foreach (var val in toggles.Where(t => t.isOn))
        //{
        //    lst.Add( val.GetComponentInChildren<Text>().text);
        //}
        UnityEngine.Color color = new UnityEngine.Color(1.0f, 1.0f, 1.0f);
        foreach (var val in toggles)
        {
            var text = String.Empty;
            if (val.isOn)
            {
                text = val.GetComponentInChildren<Text>().text;
                lst.Add(text);
                var textControl = val.GetComponentInChildren<TextMeshProUGUI>();
                textControl.text = text;
                textControl.color = color;
            }
            else
            {
                text = val.GetComponentInChildren<TextMeshProUGUI>().text;
                val.GetComponentInChildren<TextMeshProUGUI>().text = $"<s>{text}</s>";
            }   
        }
        return lst;
    }

    private void SetWheelPieceLabel(List<string> labels, PickerWheel item)
    {
        int i = 0;
        UnityEngine.Color color = new UnityEngine.Color(1.0f, 1.0f, 1.0f);
        foreach (var label in labels)
        {
            item.wheelPieces[i].Label = label;
            Text textControl = item.WheelPieceTransforms[i].GetChild(1).GetComponent<Text>();
            textControl.text = label;
            textControl.color = color;
            i++;
        }
    }

    private void TriggerCelebrations()
    {
        if(!removeCelebrations.isOn)
        {
            GameObject ob = Instantiate(confettiPrefab);
            Destroy(ob, 5f);
        }
    }
}
