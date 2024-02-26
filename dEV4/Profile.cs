namespace dEV4
{
    public class Profiles
    {
        public List<Profile> profiles { get; set; }
    }
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Year { get; set; }
    }
}
