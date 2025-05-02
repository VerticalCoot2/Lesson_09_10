using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class MySelectable : MonoBehaviour, ISelectHandler, IPointerClickHandler, IDeselectHandler
{
    //saját egységek, az összes, és a kijelöltek közülük.
    public static List<MySelectable> allMySelectables = new List<MySelectable>();
    public static List<MySelectable> currentlySelected = new List<MySelectable>();

    public Material unSelectedMat;

    public static Material SelectedMat;
    public Material selectedMatFor;

    private ParticleSystem myParticle;
    private SkinnedMeshRenderer smr;

    public bool selected;
    private void Awake()
    {
        //static ellen cheat
        SelectedMat = selectedMatFor;

        //belerakom az össz saját objektumot egy listába
        allMySelectables.Add(this);

        //objektum rendererje
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //control gombbal pluszban többet jelöl ki,
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            DeselectAll(eventData);
        }
        //anélkül csak egyet.
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //jelenleg ezt jelöltük ki, aki nem Ellenfél
        if (gameObject.tag != "Enemy")
        {
            currentlySelected.Add(this);
            selected = true;
            //máskép nézki, jelölés látszódik.
            smr.material = SelectedMat;
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //anyag vissza állítása.
        smr.material = unSelectedMat;
        selected = false;
    }
    public static void DeselectAll(BaseEventData eventData)
    {
        //Az összes eddig kijelölt elemet jelöletlenné állítjuk.

        foreach (MySelectable unit in currentlySelected)
        {
            unit.OnDeselect(eventData);
        }
        //utána tisztítjuk a jelenlegi kijelölt listát.
        currentlySelected.Clear();
    }
}
