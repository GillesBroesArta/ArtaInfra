using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtaInfra.Utils.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ArtaInfra.Utils.Pagination
{

    //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page
    public class PaginatedList<T, U> : List<U> where T : class
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public int Offset { get; }
        public int Limit { get; }
        public int Total { get; }
        public ListInfo ListInfo { get; private set; }

        public PaginatedList(List<T> source, int total, int offset, int limit)
        {
            Offset = offset; //Start position
            Limit = limit; //Amount of records to return
            Total = total; //Total records

            //TotalPages = (int)Math.Ceiling(count / (double)limit);

            //Same type no mapping needed
            if (typeof(T) == typeof(U))
            {
                if (source != null)
                {
                    AddRange((IEnumerable<U>)source);
                    CreateActionLinks();
                    return;
                }
            }
            //Dest != source, Mapping (automapper) profile needs te exist
            var retList = new List<U>();
            foreach (var item in source)
            {
                retList.Add(Mapper.Map<U>(item));
            }
            AddRange(retList);
            CreateActionLinks();
        }

        private void CreateActionLinks()
        {
            ListInfo = new ListInfo
            {
                Size = Count,
                HasMore = Count < Total,
                Count = Total
            };
            ListInfo.Links.AddSelfLink(_httpContextAccessor);
            if (Total == 0 || Total <= Limit || Limit == 0) 
            {
                ListInfo.Links.Add("first", null);
                ListInfo.Links.Add("previous", null);
                ListInfo.Links.Add("next", null);
                ListInfo.Links.Add("last", null);
                return; //Early out when no paging is possible
            }

            ListInfo.Links.AddPagingLink(_httpContextAccessor, "first", 0, Limit);
            var previous = Offset - Limit > 0 ? Offset - Limit : 0;
            if (Offset > 0) ListInfo.Links.AddPagingLink(_httpContextAccessor, "previous", previous, Limit);
            else ListInfo.Links.Add("previous", null);
            var next = Offset + Limit;
            if (next >= Total) next = Offset;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "next", next, Limit);
            var last = Total / Limit * Limit;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "last", last, Limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="source">When the query has inner joins, use GetCountFromInnerJoin first to retrieve the correct amount of results</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="countFromInnerJoin"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T, U>> CreateAsync(IHttpContextAccessor httpContextAccessor, IQueryable<T> source, int offset, int limit, int? countFromInnerJoin)
        {
            _httpContextAccessor = httpContextAccessor;

            int count;
            //BEWARE: the results can be incorrect, the CountAsync query has a bug, not using the inner join (https://github.com/aspnet/EntityFrameworkCore/issues/8201)
            if (countFromInnerJoin.HasValue)
            {
                count = countFromInnerJoin.Value;
            }
            else count = await source.CountAsync();

            if (count == 0)
                return new PaginatedList<T, U>(new List<T>(), 0, 0, 0);

            List<T> items = new List<T>();
            if (limit > 0 && offset < count)
                items = await source.Skip(offset).Take(limit).ToListAsync();
            return new PaginatedList<T, U>(items, count, offset, limit);
        }
    }
}

