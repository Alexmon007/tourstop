namespace Business.Handlers.Request
{
    /// <summary>
    /// To create queries to find entities in the database.
    /// </summary>
    public class Filter
    {
        private string _expressionString;
        public string PropertyName { get; set; }
        public string Relationship { get; set; }
        public string Value { get; set; }


        public string ExpressionString
        {
            get
            {
                return _expressionString ??
                       (_expressionString = string.Format(@"{0}{1}""{2}""", PropertyName, Relationship, Value));
            }
            set { _expressionString = value; }
        }

        public static Filter operator +(Filter x, Filter y)
        {
            var joinWord = x.PropertyName == y.PropertyName ? "OR" : "AND";
            return new Filter
            {
                ExpressionString = string.Join(joinWord, x.ExpressionString, x.ExpressionString)
            };
        }
    }
}