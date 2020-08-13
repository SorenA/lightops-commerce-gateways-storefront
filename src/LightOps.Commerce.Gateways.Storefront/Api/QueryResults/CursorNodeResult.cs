namespace LightOps.Commerce.Gateways.Storefront.Api.QueryResults
{
    public class CursorNodeResult<T>
    {
        /// <summary>
        /// The cursor of the result
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// The result node
        /// </summary>
        public T Node { get; set; }
    }
}