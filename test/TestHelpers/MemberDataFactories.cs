using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestHelpers
{
    public static class MemberDataFactories
    {
        public static class AreaInSubdomainTestData 
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

        public static class ControllerInSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "localhost", "/", "Home", "Index", "http://Home.localhost/" };
                yield return new object[] { "localhost", "/", "Home", "About", "http://Home.localhost/About" };
                yield return new object[] { "localhost", "/", "Test", "Index", "http://Test.localhost/" };
                yield return new object[] { "localhost", "/", "Test", "About", "http://Test.localhost/About" };
                yield return new object[] { "home.localhost", "/",  "Home", "Index", "/" };
                yield return new object[] { "home.localhost", "/",  "Home", "About", "/About" };
                yield return new object[] { "test.localhost", "/",  "Test", "Index", "/" };
                yield return new object[] { "test.localhost", "/",  "Test", "About", "/About" };
            }
        }

        public static class ConstantSubdomainTestData
        { 
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "localhost", "/", "Home", "Index", "http://constantsubdomain.localhost/" };
                yield return new object[] { "localhost", "/", "Home", "About", "http://constantsubdomain.localhost/Home/About" };
                yield return new object[] { "localhost", "/", "Test", "Index", "http://constantsubdomain.localhost/Test" };
                yield return new object[] { "localhost", "/", "Test", "About", "http://constantsubdomain.localhost/Test/About" };
                yield return new object[] { "constantsubdomain.localhost", "/",  "Home", "Index", "/" };
                yield return new object[] { "constantsubdomain.localhost", "/",  "Home", "About", "/Home/About" };
                yield return new object[] { "constantsubdomain.localhost", "/",  "Test", "Index", "/Test" };
                yield return new object[] { "constantsubdomain.localhost", "/",  "Test", "About", "/Test/About" };
            }
        }
    }
}
