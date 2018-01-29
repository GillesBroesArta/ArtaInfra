using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Arta.Features.DataContracts;
using Arta.Infrastructure.Helpers;

namespace Arta.Infrastructure
{
    //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page
    public class PaginatedList<T, U> : List<U>
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public int Offset { get; private set; }
        public int Limit { get; private set; }
        public int Total { get; private set; }
        public ListInfo ListInfo { get; private set; }

        public PaginatedList(List<T> source, int total, int offset, int limit)
        {
            Offset = offset; //Van waar beginnen we
            Limit = limit; //hoeveel nemen we er
            Total = total; //hoeveel zijn er in totaal

            //TotalPages = (int)Math.Ceiling(count / (double)limit);

            //Same type no mapping needed
            if (typeof(T) == typeof(U))
            {
                if (source != null)
                {
                    this.AddRange((IEnumerable<U>)source);
                    return;
                }
            }
            //Dest != source, Mapping (automapper) profile needs te exist
            var retList = new List<U>();
            foreach (var item in source)
            {
                retList.Add(Mapper.Map<U>(item));
            }
            this.AddRange(retList);
            CreateActionLinks();
        }

        private void CreateActionLinks()
        {
            ListInfo = new ListInfo
            {
                Size = this.Count,
                HasMore = this.Count < Total,
                Count = this.Total
            };
            ListInfo.Links.AddSelfLink(_httpContextAccessor);
            if (Total == 0 || Total <= Limit) return; //Early out when no paging is possible

            ListInfo.Links.AddPagingLink(_httpContextAccessor, "first", 0, Limit);
            var previous = Offset - Limit;
            if (previous < 0) previous = Offset;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "previous", previous, Limit);
            var next = Offset + Limit;
            if (next >= Total) next = Offset;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "next", next, Limit);
            var last = (int)(Total / Limit) * Limit;
            ListInfo.Links.AddPagingLink(_httpContextAccessor, "last", last, Limit);
        }

        public static async Task<PaginatedList<T, U>> CreateAsync(IHttpContextAccessor httpContextAccessor, IQueryable<T> source, int offset, int limit, int? countFromInnerJoin)
        {
            _httpContextAccessor = httpContextAccessor;

            int count = 0;
            //BEWARE: the results can be incorrect, the CountAsync query has a bug, not using the inner join (https://github.com/aspnet/EntityFrameworkCore/issues/8201)
            if (countFromInnerJoin.HasValue)
            {
                count = countFromInnerJoin.Value;
            }
            else count = await source.CountAsync();

            if (count == 0)
                return new PaginatedList<T, U>(new List<T>(), 0, 0, 0);

            var items = await source.Skip(offset).Take(limit).ToListAsync();
            return new PaginatedList<T, U>(items, count, offset, limit);
        }
    }
}


