using System.Collections;

namespace Itmo.Dev.Asap.Google.Common.Collections;

public class DynamicReadonlyList<TInnerValue, TOuterValue> : IReadOnlyList<TOuterValue>
{
    private readonly IReadOnlyList<TInnerValue> _list;
    private readonly Func<TInnerValue, TOuterValue> _mapper;

    public DynamicReadonlyList(IReadOnlyList<TInnerValue> list, Func<TInnerValue, TOuterValue> mapper)
    {
        _list = list;
        _mapper = mapper;
    }

    public int Count => _list.Count;

    public TOuterValue this[int index] => _mapper.Invoke(_list[index]);

    public IEnumerator<TOuterValue> GetEnumerator()
    {
        return _list.Select(_mapper).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}