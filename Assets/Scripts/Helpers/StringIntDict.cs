
using System.Collections.Generic;

public class StringIntDict
{
    public event System.Action Changed;
    private Dictionary<string, int> dict = new Dictionary<string, int>();
    public int this[string key]
    {
        get => dict.ContainsKey(key) ? dict[key] : 0;
        set
        {
            dict[key] = value;
            Changed?.Invoke();
        }
    }
}