namespace Itmo.Dev.Asap.Google.Common.Collections;

public static class CollectionFactory
{
    public static IReadOnlyDictionary<TKey, TOuterValue> CreateDictionary<TKey, TOuterValue, TInnerValue>(
        IReadOnlyDictionary<TKey, TInnerValue> dictionary,
        Func<TInnerValue, TOuterValue> mapper)
    {
        return new DynamicReadonlyDictionary<TKey, TInnerValue, TOuterValue>(dictionary, mapper);
    }

    public static IReadOnlyList<TOuterValue> CreateList<TInnerValue, TOuterValue>(
        IReadOnlyList<TInnerValue> list,
        Func<TInnerValue, TOuterValue> mapper)
    {
        return new DynamicReadonlyList<TInnerValue, TOuterValue>(list, mapper);
    }
}