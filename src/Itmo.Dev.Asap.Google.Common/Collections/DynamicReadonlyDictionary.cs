using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Itmo.Dev.Asap.Google.Common.Collections;

public class DynamicReadonlyDictionary<TKey, TInnerValue, TOuterValue> : IReadOnlyDictionary<TKey, TOuterValue>
{
    private readonly IReadOnlyDictionary<TKey, TInnerValue> _dictionary;
    private readonly Func<TInnerValue, TOuterValue> _mapper;

    public DynamicReadonlyDictionary(
        IReadOnlyDictionary<TKey, TInnerValue> dictionary,
        Func<TInnerValue, TOuterValue> mapper)
    {
        _dictionary = dictionary;
        _mapper = mapper;
    }

    public int Count => _dictionary.Count;

    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public IEnumerable<TOuterValue> Values => _dictionary.Values.Select(_mapper);

    public TOuterValue this[TKey key] => _mapper.Invoke(_dictionary[key]);

    public IEnumerator<KeyValuePair<TKey, TOuterValue>> GetEnumerator()
    {
        return _dictionary
            .Select(innerValue => new KeyValuePair<TKey, TOuterValue>(innerValue.Key, _mapper.Invoke(innerValue.Value)))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TOuterValue value)
    {
        if (_dictionary.TryGetValue(key, out TInnerValue? innerValue))
        {
            value = _mapper.Invoke(innerValue);
            return true;
        }

        value = default;
        return false;
    }
}