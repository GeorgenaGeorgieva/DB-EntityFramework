namespace SoftUni
{
    using System;
    using System.Linq;
    using SoftUni.Data;
    
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = RemoveTown(context);
            Console.WriteLine(result);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townForDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addressForDelete = context.Addresses
                .Where(a => a.Town.Name == "Seattle");

            var countOfDeletedAddresses = addressForDelete.Count();

            context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList()
                .ForEach(a => a.AddressId = null);

            context.Addresses.RemoveRange(addressForDelete);
            context.Towns.Remove(townForDelete);

            context.SaveChanges();

            return $"{countOfDeletedAddresses} addresses in Seattle were deleted";
        }
    }
}
