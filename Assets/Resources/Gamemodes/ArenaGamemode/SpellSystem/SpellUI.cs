using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    public GameObject[] spellSlots;
    public GameObject[] spellOverlay;
    [SerializeField]
    private Sprite emptyIcon;
    [SerializeField]
    private Color emptyColor;
    [SerializeField]
    private Color selectedColor;

    int selectedSpell = 0;
    // Start is called before the first frame update

    public void SetSpell(int index, Sprite icon)
    {
        spellSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = icon;
    }

    public void SetSelectedSpell(int index)
    {
        spellOverlay[selectedSpell].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        spellOverlay[selectedSpell].GetComponent<UnityEngine.UI.Image>().color = selectedColor;
    }

    public void ClearSpell(int index)
    {
        spellSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = emptyIcon;
    }
}
