namespace DevOpen.Domain.Model.LoanApplications.Events
{  
    [EventType("InvoiceAddressRegistered")]
    public class InvoiceAddressRegistered : LoanApplicationDomainEvent
    {
        public string Street { get; }
        public string Street2 { get; }
        public string PostalCode { get; }
        public string City { get; }
        public string Country { get; }
        public string CareOf { get; }
        
        public InvoiceAddressRegistered(string street, string street2, string postalCode, string city, string country, string careOf)
        {
            Street = street;
            Street2 = street2;
            PostalCode = postalCode;
            City = city;
            Country = country;
            CareOf = careOf;
        }

        internal InvoiceAddressRegistered(Address address) : 
            this(address.Street, address.Street2, address.PostalCode, address.City, address.Country, address.CareOf)
        {
        }
    }
}