// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using NUnit.Framework;

namespace CppAst.Tests
{
    public class TestMethods : InlineTestBase
    {
        [Test]
        public void TestSimple()
        {
            ParseAssert(@"
class MyClass0
{
    public:
    void method0();

    private:
    static void method1();
};
",
                compilation =>
                {
                    Assert.False(compilation.HasErrors);

                    Assert.AreEqual(1, compilation.Classes.Count);

                    var cppClass = compilation.Classes[0];
                    Assert.AreEqual("MyClass0", cppClass.Name);

                    var methods = cppClass.Functions;
                    Assert.AreEqual(2, methods.Count);

                    Assert.AreEqual("public void method0()", methods[0].ToString());
                    Assert.AreEqual("private static void method1()", methods[1].ToString());
                }
            );
        }

        [Test]
        public void TestElaboratedType()
        {
            ParseAssert(@"
namespace MyNamespace
{
class AnotherClass
{

};

class MyStruct
{

};

typedef AnotherClass MyNewType;
}

class SameNamespaceClass
{

};

class MyClass0
{
    public:

    const MyNamespace::AnotherClass & method0();

    const class SameNamespaceClass & method1();

    const MyNamespace::MyNewType & method2();

    const MyNamespace::MyStruct & method3();
};
",
                compilation =>
                {
                    Assert.False(compilation.HasErrors);

                    Assert.AreEqual(2, compilation.Classes.Count);

                    var cppClass = compilation.Classes[1];
                    Assert.AreEqual("MyClass0", cppClass.Name);

                    var methods = cppClass.Functions;
                    Assert.AreEqual(4, methods.Count);

                    Assert.AreEqual("public const AnotherClass& method0()", methods[0].ToString());
                    Assert.AreEqual("public const SameNamespaceClass& method1()", methods[1].ToString());
                    Assert.AreEqual("public const MyNewType& method2()", methods[2].ToString());
                    Assert.AreEqual("public const MyStruct& method3()", methods[3].ToString());
                }
            );
        }
    }
}