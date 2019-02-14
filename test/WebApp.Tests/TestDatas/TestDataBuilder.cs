using WebApp.EntityFrameworkCore;

namespace WebApp.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly WebAppDbContext _context;

        public TestDataBuilder(WebAppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            //create test data here...
        }
    }
}