namespace MyFirstWebApi.Models
{
    public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public PaginatedList(List<T> items,int pageIndex, int count,int pageSize)
        {
            PageIndex = pageIndex;
            TotalCount = (int)Math.Ceiling(count/(double)pageSize);
            AddRange(items);
        }

        public static PaginatedList<T> Create (IQueryable<T> source,int pageIndex,int pageSize)
        {
            var count = source.Count();
            var item = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(item, pageIndex, count, pageSize);
        }

    }
}
