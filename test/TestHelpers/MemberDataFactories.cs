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
                yield return new object[] { "example.com", "/", "area1", "Home", "Index", "http://area1.example.com/" };
                yield return new object[] { "example.com", "/", "area1", "Home", "About", "http://area1.example.com/Home/About" };
                yield return new object[] { "example.com", "/", "area1", "Test", "Index", "http://area1.example.com/Test" };
                yield return new object[] { "example.com", "/", "area1", "Test", "About", "http://area1.example.com/Test/About" };
                yield return new object[] { "area1.example.com", "/", "area1", "Home", "Index", "/" };
                yield return new object[] { "area1.example.com", "/", "area1", "Home", "About", "/Home/About" };
                yield return new object[] { "area1.example.com", "/", "area1", "Test", "Index", "/Test" };
                yield return new object[] { "area1.example.com", "/", "area1", "Test", "About", "/Test/About" };
            }
        }

        public static class ControllerInSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "example.com", "/", "Home", "Index", "http://Home.example.com/" };
                yield return new object[] { "example.com", "/", "Home", "About", "http://Home.example.com/About" };
                yield return new object[] { "example.com", "/", "Test", "Index", "http://Test.example.com/" };
                yield return new object[] { "example.com", "/", "Test", "About", "http://Test.example.com/About" };
                yield return new object[] { "home.example.com", "/",  "Home", "Index", "/" };
                yield return new object[] { "home.example.com", "/",  "Home", "About", "/About" };
                yield return new object[] { "test.example.com", "/",  "Test", "Index", "/" };
                yield return new object[] { "test.example.com", "/",  "Test", "About", "/About" };
            }
        }

        public static class ConstantSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "example.com", "/", "Home", "Index", "http://constantsubdomain.example.com/" };
                yield return new object[] { "example.com", "/", "Home", "About", "http://constantsubdomain.example.com/Home/About" };
                yield return new object[] { "example.com", "/", "Test", "Index", "http://constantsubdomain.example.com/Test" };
                yield return new object[] { "example.com", "/", "Test", "About", "http://constantsubdomain.example.com/Test/About" };
                yield return new object[] { "constantsubdomain.example.com", "/", "Home", "Index", "/" };
                yield return new object[] { "constantsubdomain.example.com", "/", "Home", "About", "/Home/About" };
                yield return new object[] { "constantsubdomain.example.com", "/", "Test", "Index", "/Test" };
                yield return new object[] { "constantsubdomain.example.com", "/", "Test", "About", "/Test/About" };
            }
        }

        public static class W3AreaInSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "www.example.com", "/", "area1", "Home", "Index", "http://www.area1.example.com/" };
                yield return new object[] { "www.example.com", "/", "area1", "Home", "About", "http://www.area1.example.com/Home/About" };
                yield return new object[] { "www.example.com", "/", "area1", "Test", "Index", "http://www.area1.example.com/Test" };
                yield return new object[] { "www.example.com", "/", "area1", "Test", "About", "http://www.area1.example.com/Test/About" };
                yield return new object[] { "www.area1.example.com", "/", "area1", "Home", "Index", "/" };
                yield return new object[] { "www.area1.example.com", "/", "area1", "Home", "About", "/Home/About" };
                yield return new object[] { "www.area1.example.com", "/", "area1", "Test", "Index", "/Test" };
                yield return new object[] { "www.area1.example.com", "/", "area1", "Test", "About", "/Test/About" };
            }
        }

        public static class W3ControllerInSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "www.example.com", "/", "Home", "Index", "http://www.Home.example.com/" };
                yield return new object[] { "www.example.com", "/", "Home", "About", "http://www.Home.example.com/About" };
                yield return new object[] { "www.example.com", "/", "Test", "Index", "http://www.Test.example.com/" };
                yield return new object[] { "www.example.com", "/", "Test", "About", "http://www.Test.example.com/About" };
                yield return new object[] { "www.home.example.com", "/", "Home", "Index", "/" };
                yield return new object[] { "www.home.example.com", "/", "Home", "About", "/About" };
                yield return new object[] { "www.test.example.com", "/", "Test", "Index", "/" };
                yield return new object[] { "www.test.example.com", "/", "Test", "About", "/About" };
            }
        }

        public static class W3ConstantSubdomainTestData
        {
            public static IEnumerable<object[]> Generate()
            {
                yield return new object[] { "www.example.com", "/", "Home", "Index", "http://www.constantsubdomain.example.com/" };
                yield return new object[] { "www.example.com", "/", "Home", "About", "http://www.constantsubdomain.example.com/Home/About" };
                yield return new object[] { "www.example.com", "/", "Test", "Index", "http://www.constantsubdomain.example.com/Test" };
                yield return new object[] { "www.example.com", "/", "Test", "About", "http://www.constantsubdomain.example.com/Test/About" };
                yield return new object[] { "www.constantsubdomain.example.com", "/", "Home", "Index", "/" };
                yield return new object[] { "www.constantsubdomain.example.com", "/", "Home", "About", "/Home/About" };
                yield return new object[] { "www.constantsubdomain.example.com", "/", "Test", "Index", "/Test" };
                yield return new object[] { "www.constantsubdomain.example.com", "/", "Test", "About", "/Test/About" };
            }
        }
    }
}
