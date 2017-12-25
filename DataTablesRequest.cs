using System.Collections.Generic;

namespace ServerSideDataTable
{
    public class DataTablesRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; } = new Search();
        public List<Order> Order { get; set; } = new List<Order>();
        public IEnumerable<Column> Columns { get; set; }
        public IDictionary<string, object> AdditionalParameters { get; set; }
    }

    public class Sort
    {
        public SortDirection Dir { get; set; }
        public int Column { get; set; }

        public void SetSortDirection(string direction)
        {
            if (direction.Equals("asc"))
            {
                this.Dir = SortDirection.Asc;
            }
            else
            {
                this.Dir = SortDirection.Desc;
            }
        }
    }

    public class Search
    {
        public string Value { get; set; }
        public bool IsRegex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class Column
    {
        public bool SetSort(int order, int direction)
        {
            Sort.Column = order;
            Sort.Dir = direction == 0 ? SortDirection.Asc : SortDirection.Desc;
            return IsSortable;
        }

        public string Name { get; set; }
        public string Field { get; set; }
        public bool IsSearchable { get; set; }
        public Search Search { get; set; }
        public bool IsSortable { get; set; }
        public Sort Sort { get; set; }
    }

    public enum SortDirection
    {
        Asc = 0,
        Desc = 1
    }
}
