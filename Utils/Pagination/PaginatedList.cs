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
                HasMore = Count < Total ? "Yes" : "No",
                Count = Total
            };
            ListInfo.Links.AddSelfLink(_httpContextAccessor);
            if (Total == 0 || Total <= Limit || Limit == 0) 
            {
                ListInfo.Links.Add("First", null);
                ListInfo.Links.Add("Previous", null);
                ListInfo.Links.Add("Next", null);
                ListInfo.Links.Add("Last", null);
                return; //Early out when no paging is possible
            }

            ListInfo.Links.AddPagingLink(_httpContextAccessor, "First", 0, Limit);
            var previous = Offset - Limit > 0 ? Offset - Limit : 0;
            if (Offset > 0) ListInfo.Links.AddPagingLink(_httpContextAccessor, "Previous", previous, Limit);
            else ListInfo.Links.Add("Previous", null);
            var next = Offset + Limit;
            if (next >= Total) next = Offset;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "Next", next, Limit);
            var last = Total / Limit * Limit;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "Last", last, Limit);
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

            //Use defaults value according to generic specs 
	        if (limit < 0) limit = 100;
            if (offset < 0) offset = 0;

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

        /// <summary>
        /// Put an already existing list in the paginated form
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static PaginatedList<T, U> UseList(IHttpContextAccessor httpContextAccessor, IList<T> source, int offset, int limit, int totalCount)
        {
            _httpContextAccessor = httpContextAccessor;

            //Use defaults value according to generic specs 
            if (limit < 0) limit = 100;
            if (offset < 0) offset = 0;

            var count = source.Count;
            if (count == 0 && totalCount == 0)
                return new PaginatedList<T, U>(new List<T>(), 0, 0, 0);

            var items = source.ToList();
            return new PaginatedList<T, U>(items, totalCount, offset, limit);
        }
    }
}

