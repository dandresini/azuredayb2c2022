namespace azuredayfunctiontest.custombinding
{
    public class MyUserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; } = false;
    }
}
