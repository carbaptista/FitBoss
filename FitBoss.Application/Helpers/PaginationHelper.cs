using System;
using System.Collections.Generic;

namespace Application.Helpers;

public class PaginationResult
{
    public int Current { get; set; }
    public int? Prev { get; set; }
    public int? Next { get; set; }
    public List<object>? Items { get; set; }
}

public static class PaginationHelper
{
    public static PaginationResult? Paginate(int current, int max)
    {
        if (current == 0 || max == 0)
            return null;

        int? prev = current == 1 ? null : current - 1;
        int? next = current == max ? null : current + 1;
        List<object> items = new List<object> { 1 };

        if (current == 1 && max == 1)
            return new PaginationResult { Current = current, Prev = prev, Next = next, Items = items };

        if (current > 4)
            items.Add("...");

        int r = 2, r1 = current - r, r2 = current + r;

        for (int i = r1 > 2 ? r1 : 2; i <= Math.Min(max, r2); i++)
            items.Add(i);

        if (r2 + 1 < max)
            items.Add("...");

        if (r2 < max)
            items.Add(max);

        return new PaginationResult { Current = current, Prev = prev, Next = next, Items = items };
    }
}

