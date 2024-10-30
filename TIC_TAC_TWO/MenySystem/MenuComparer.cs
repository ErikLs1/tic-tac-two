namespace MenySystem;

public class MenuItemComparer : IEqualityComparer<MenuItem>
{
    public bool Equals(MenuItem x, MenuItem y)
    {
        return x.Shortcut == y.Shortcut;
    }

    public int GetHashCode(MenuItem obj)
    {
        return obj.Shortcut.GetHashCode();
    }
}