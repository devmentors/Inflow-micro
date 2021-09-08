﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Users.Core.DAL
{
    internal static class Extensions
    {
        public static Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> data, IPagedQuery query)
            => data.PaginateAsync(query.Page, query.Results);

        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> data, int page, int results)
        {
            if (page <= 0)
            {
                page = 1;
            }

            results = results switch
            {
                <= 0 => 10,
                > 100 => 100,
                _ => results
            };

            var totalResults = await data.CountAsync();
            var totalPages = totalResults <= results ? 1 : (int) Math.Floor((double) totalResults / results);
            var result = await data.Skip((page - 1) * results).Take(results).ToListAsync();

            return PagedResult<T>.Create(result, page, results, totalPages, totalResults);
        }

        public static Task<List<T>> SkipAndTakeAsync<T>(this IQueryable<T> data, IPagedQuery query)
            => data.SkipAndTakeAsync(query.Page, query.Results);

        public static async Task<List<T>> SkipAndTakeAsync<T>(this IQueryable<T> data, int page, int results,
            CancellationToken cancellationToken = default)
        {
            if (page <= 0)
            {
                page = 1;
            }

            results = results switch
            {
                <= 0 => 10,
                > 100 => 100,
                _ => results
            };

            return await data.Skip((page - 1) * results).Take(results).ToListAsync(cancellationToken);
        }
    }
}