using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestHelpers
{
    public static class MemberDataFactories
    {
        public static class AreaSubdomainTestData 
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "localhost", "/", "area1", "Home", "Index", "http://area1.localhost/" };
                yield return new object[] { "localhost", "/", "area1", "Home", "About", "http://area1.localhost/Home/About" };
                yield return new object[] { "localhost", "/", "area1", "Test", "Index", "http://area1.localhost/Test" };
                yield return new object[] { "localhost", "/", "area1", "Test", "About", "http://area1.localhost/Test/About" };
                yield return new object[] { "area1.localhost", "/", "area1", "Home", "Index", "/" };
                yield return new object[] { "area1.localhost", "/", "area1", "Home", "About", "/Home/About" };
                yield return new object[] { "area1.localhost", "/", "area1", "Test", "Index", "/Test" };
                yield return new object[] { "area1.localhost", "/", "area1", "Test", "About", "/Test/About" };
            }
        }
    }
}
