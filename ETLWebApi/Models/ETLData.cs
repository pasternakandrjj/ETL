namespace ETLWebApi.Models
{
    public class ETLData
    {
        public int Id { get; set; }

        public DateTime PickupDatetime { get; set; }

        public DateTime DropoffDatetime { get; set; }

        public int PassengerCount { get; set; }

        public double TripDistance { get; set; }

        public string? StoreAndFwdFlag { get; set; }

        public int PULocationID { get; set; }

        public int DOLocationID { get; set; }

        public double FareAmount { get; set; }

        public double TipAmount { get; set; }
    }
}