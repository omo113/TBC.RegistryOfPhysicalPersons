﻿namespace TBC.ROPP.Domain.Shared;

public class DomainResult<TResult, TRight>
{
    private readonly TResult _left;
    private readonly TRight _right;
    private readonly bool _isLeft;

    public DomainResult(TResult left)
    {
        _left = left;
        _isLeft = true;
    }

    public DomainResult(TRight right)
    {
        _right = right;
        _isLeft = false;
    }

    public T Match<T>(Func<TResult, T> left, Func<TRight, T> right)
    {
        return _isLeft ? left(_left) : right(_right);
    }

    public async Task<T> MatchAsync<T>(Func<TResult, Task<T>> left, Func<TRight, Task<T>> right)
    {
        return _isLeft ? await left(_left) : await right(_right);
    }
}