using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventSummary : IEventSummary
{
    private Dictionary<string, IEventSummarySeries> _this = new Dictionary<string, IEventSummarySeries>();

    public void Add(string key, IEventSummarySeries value)
    {
        _this.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _this.ContainsKey(key);
    }

    public ICollection<string> Keys
    {
        get { return _this.Keys; }
    }

    public bool Remove(string key)
    {
        return _this.Remove(key);
    }

    public bool TryGetValue(string key, out IEventSummarySeries value)
    {
        return _this.TryGetValue(key, out value);
    }

    public ICollection<IEventSummarySeries> Values
    {
        get { return _this.Values; }
    }

    public IEventSummarySeries this[string key]
    {
        get
        {
            return _this[key];
        }
        set
        {
            _this[key] = value;
        }
    }

    public void Add(KeyValuePair<string, IEventSummarySeries> item)
    {
        _this.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _this.Clear();
    }

    public bool Contains(KeyValuePair<string, IEventSummarySeries> item)
    {
        return _this.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, IEventSummarySeries>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public int Count
    {
        get { return _this.Count; }
    }

    public bool IsReadOnly
    {
        get { throw new NotImplementedException(); }
    }

    public bool Remove(KeyValuePair<string, IEventSummarySeries> item)
    {
        return _this.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, IEventSummarySeries>> GetEnumerator()
    {
        return _this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _this.GetEnumerator();
    }
}

public class MatchSummary : IMatchSummary
{
    private List<IMatchSummaryItem> _this = new List<IMatchSummaryItem>();

    public DateTime ScheduledDateTime {get; set;}

    public int IndexOf(IMatchSummaryItem item)
    {
        return _this.IndexOf(item);
    }

    public void Insert(int index, IMatchSummaryItem item)
    {
        _this.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _this.RemoveAt(index);
    }

    public IMatchSummaryItem this[int index]
    {
        get
        {
            return _this[index];
        }
        set
        {
            _this[index] = value;
        }
    }

    public void Add(IMatchSummaryItem item)
    {
        _this.Add(item);
    }

    public void Clear()
    {
        _this.Clear();
    }

    public bool Contains(IMatchSummaryItem item)
    {
        return _this.Contains(item);
    }

    public void CopyTo(IMatchSummaryItem[] array, int arrayIndex)
    {
        _this.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return _this.Count; }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
            //_this.IsRe
        }
    }

    public bool Remove(IMatchSummaryItem item)
    {
        return _this.Remove(item);
    }

    public IEnumerator<IMatchSummaryItem> GetEnumerator()
    {
        return _this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _this.GetEnumerator();
    }
}

public class MatchSummarySeries : IMatchSummarySeries
{
    private Dictionary<DateTime, IMatchSummary> _this = new Dictionary<DateTime, IMatchSummary>();

    public string Series { get; set; }

    public void Add(DateTime key, IMatchSummary value)
    {
        _this.Add(key, value);
    }

    public bool ContainsKey(DateTime key)
    {
        return _this.ContainsKey(key);
    }

    public ICollection<DateTime> Keys
    {
        get { return _this.Keys; }
    }

    public bool Remove(DateTime key)
    {
        return _this.Remove(key);
    }

    public bool TryGetValue(DateTime key, out IMatchSummary value)
    {
        return _this.TryGetValue(key, out value);
    }

    public ICollection<IMatchSummary> Values
    {
        get { return _this.Values; }
    }

    public IMatchSummary this[DateTime key]
    {
        get
        {
            return _this[key];
        }
        set
        {
            _this[key] = value;
        }
    }

    public void Add(KeyValuePair<DateTime, IMatchSummary> item)
    {
        _this.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _this.Clear();
    }

    public bool Contains(KeyValuePair<DateTime, IMatchSummary> item)
    {
        return _this.Contains(item);
    }

    public void CopyTo(KeyValuePair<DateTime, IMatchSummary>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public int Count
    {
        get { return _this.Count; }
    }

    public bool IsReadOnly
    {
        get { throw new NotImplementedException(); }
    }

    public bool Remove(KeyValuePair<DateTime, IMatchSummary> item)
    {
        return _this.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<DateTime, IMatchSummary>> GetEnumerator()
    {
        return _this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _this.GetEnumerator();
    }
}

public class MatchSummaryItem : IMatchSummaryItem
{
    public int Rank { get; set; }

    public string Team { get; set; }

    public string Color { get; set; }

    public int TeamScore { get; set; }

    public int RankPoints { get; set; }
}

public class EventMatchSummary : IEventMatchSummary
{
    private Dictionary<string, IMatchSummarySeries> _this = new Dictionary<string, IMatchSummarySeries>();

    public void Add(string key, IMatchSummarySeries value)
    {
        _this.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _this.ContainsKey(key);
    }

    public ICollection<string> Keys
    {
        get { return _this.Keys; }
    }

    public bool Remove(string key)
    {
        return _this.Remove(key);
    }

    public bool TryGetValue(string key, out IMatchSummarySeries value)
    {
        return _this.TryGetValue(key, out value);
    }

    public ICollection<IMatchSummarySeries> Values
    {
        get { return _this.Values; }
    }

    public IMatchSummarySeries this[string key]
    {
        get
        {
            return _this[key];
        }
        set
        {
            _this[key] = value;
        }
    }

    public void Add(KeyValuePair<string, IMatchSummarySeries> item)
    {
        _this.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _this.Clear();
    }

    public bool Contains(KeyValuePair<string, IMatchSummarySeries> item)
    {
        return _this.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, IMatchSummarySeries>[] array, int arrayIndex)
    {
        throw new NotImplementedException(); 
        //_this.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return _this.Count; }
    }

    public bool IsReadOnly
    {
        get { throw new NotImplementedException(); }
    }

    public bool Remove(KeyValuePair<string, IMatchSummarySeries> item)
    {
        return _this.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, IMatchSummarySeries>> GetEnumerator()
    {
        return _this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _this.GetEnumerator();
    }
}

public class EventSummarySeries : IEventSummarySeries
{
    private List<IEventSummaryItem> _this = new List<IEventSummaryItem>();
    private string series = "";
    public string Series
    {
        get
        {
            return series;
        }
        set
        {
            series = value;
        }
    }

    public int IndexOf(IEventSummaryItem item)
    {
        return _this.IndexOf(item);
    }

    public void Insert(int index, IEventSummaryItem item)
    {
        _this.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _this.RemoveAt(index);
    }

    public IEventSummaryItem this[int index]
    {
        get
        {
            return _this[index];
        }
        set
        {
            _this[index] = value;
        }
    }

    public void Add(IEventSummaryItem item)
    {
        _this.Add(item);
    }

    public void Clear()
    {
        _this.Clear();
    }

    public bool Contains(IEventSummaryItem item)
    {
        return _this.Contains(item);
    }

    public void CopyTo(IEventSummaryItem[] array, int arrayIndex)
    {
        _this.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return _this.Count; }
    }

    public bool IsReadOnly
    {
        get {
            return false;
            //_this.IsRe
        }
    }

    public bool Remove(IEventSummaryItem item)
    {
        return _this.Remove(item);
    }

    public IEnumerator<IEventSummaryItem> GetEnumerator()
    {
        return _this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _this.GetEnumerator();
    }
}
public class EventSummaryItem : IEventSummaryItem
{
    public int Rank { get; set; }

    public string Team { get; set; }

    public int GamesPlayed { get; set; }

    public int TournamentPoints { get; set; }

    public double TeamScore { get; set; }
}
