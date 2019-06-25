using DevTask.MRZService.API.Services;
using System;
using Xunit;

namespace DevTask.MrzUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void get_data_from_abbyyserver()
        {
            ABBYYMRZService service = new ABBYYMRZService();
            service.GetPersonInformation(string.Empty);
        }
    }
}
