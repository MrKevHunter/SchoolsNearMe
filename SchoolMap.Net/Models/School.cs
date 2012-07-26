namespace SchoolMap.Net.Models
{
    public class School
    {
        public string SchoolName { get; set; }

        public string PostCode { get; set; }

        public string Id { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string SchoolType { get; set; }

        public string Website { get; set; }

        public Coordinate Location { get; set; }

        public OfstedRating OfstedRating { get; set; }

        public bool IsSchoolClosed
        {
            get;
            set;
        }

        public override string ToString()
        {
            return SchoolName + ", " + Town;
        }

        public string GetAddress()
        {
            return string.Format("{0},{1},{2},{3}", SchoolName, Street, Town, PostCode);
        }
    }
}