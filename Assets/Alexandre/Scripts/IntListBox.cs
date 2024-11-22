using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The box used for displaying the content
// Must inherit from the class `ListBox`
public class IntListBox : ListBox
{
    [SerializeField]
    private TMP_Text _contentText;

    // This function is invoked by the `CircularScrollingList` for updating the list content.
    protected override void UpdateDisplayContent(IListContent listContent)
    {
        var content = (IntListBank.Content)listContent;
        _contentText.text = content.Value.ToString();
    }
}