namespace Demo
{
    public class BusinessEntity
    {
        public string CountryIso2 { get; }  // snp12 Group and count sponsors by country code
     
        public BusinessEntity(string countryIso2)
        {
            this.CountryIso2 = countryIso2;             // snp58 end
        }
    }
}