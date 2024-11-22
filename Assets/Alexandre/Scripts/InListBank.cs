using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.ContentManagement;

// The bank for providing the content for the box to display
// Must be inherit from the class BaseListBank
public class IntListBank : BaseListBank
{
    // The content to be passed to the list box
    // must inherit from the class `IListContent`.
    public class Content : IListContent
    {
        public int Value;
    }

    private readonly int[] _contents = {
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10
    };

    // This function will be invoked by the `CircularScrollingList`
    // to get the content to display.
    public override IListContent GetListContent(int index)
    {
        var content = new Content {
            Value = _contents[index]
        };

        return content;
    }

    public override int GetContentCount()
    {
        return _contents.Length;
    }
}