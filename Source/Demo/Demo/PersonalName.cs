using System;

namespace Demo
{
    public class PersonalName
    {
        public string FirstName { get; }
        public string LastName { get; }  // snp26 Allow null value

        public PersonalName(string firstName, string lastName)  // snp20 This now means non-null strings
        {
            this.FirstName = firstName;  // snp09 Guard against null
            this.LastName = lastName;    // snp09 end
        }
    }
}