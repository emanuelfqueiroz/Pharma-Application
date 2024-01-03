namespace PharmaRep.Infra.Persistence.Common
{
    internal class SqlBuilder(string baseQuery, string? orderBy = null) : List<string>
    {
        public string BaseQuery { get; set; } = baseQuery;
        public string? SortFields { get; private set; } = orderBy;

        public SqlBuilder AddIf(bool condition, string query)
        {
            if (condition)
            {
                Add(query);
            }
            return this;
        }
        public SqlBuilder AddIf(string? parameter, Func<string> query)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                Add(query());
            }
            return this;
        }
        public SqlBuilder AddIf(int? parameter, Func<string> query)
        {
            if (parameter != null)
            {
                Add(query());
            }
            return this;
        }

        public SqlBuilder AddIf(bool condition, Func<string> query)
        {
            if (condition)
            {
                Add(query());
            }
            return this;
        }

        public SqlBuilder SetOrderStatement(string sortFields)
        {
            SortFields = sortFields;
            return this;
        }

        public override string ToString()
        {
            if (Count == 0)
            {
                return BaseQuery + GetOrderByStatement();
            }
            else
            {
                return $"{BaseQuery} WHERE " + string.Join(" AND ", this) + GetOrderByStatement();
            }
        }

        private string GetOrderByStatement() => string.IsNullOrEmpty(SortFields) ? "" : " ORDER BY " + SortFields;
    }
}
