namespace FunctionApp2
{
    public class DocEntity
    {
        public string PersonName { get; set; }
        public string PersonId { get; set; }

        //Unique Identifiers for Azure Tables
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
