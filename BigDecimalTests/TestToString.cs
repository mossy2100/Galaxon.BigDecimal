using Galaxon.Core.Strings;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestToString
{
    [TestMethod]
    public void TestToStringD()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("D");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("D");
        Assert.AreEqual("12345E+67", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("D");
        Assert.AreEqual("12345E-67", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("D");
        Assert.AreEqual("-12345E+67", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("D");
        Assert.AreEqual("-12345E-67", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("D");
        Assert.AreEqual(
             "314159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068E-99", str);
    }

    [TestMethod]
    public void TestToStringDU()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("DU");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("DU");
        Assert.AreEqual("12345×10⁶⁷", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("DU");
        Assert.AreEqual("12345×10⁻⁶⁷", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("DU");
        Assert.AreEqual("-12345×10⁶⁷", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("DU");
        Assert.AreEqual("-12345×10⁻⁶⁷", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("DU");
        Assert.AreEqual("31415926535897932384626433832795028841971693993751058209749445923078164"
            + "06286208998628034825342117068×10⁻" + "99".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringE()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+004", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+071", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E-063", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E+071", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E-063", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E");
        Assert.AreEqual("3.1415926535897932384626433832795028841971693993751058209749445923078164"
            + "06286208998628034825342117068E+000", str);
    }

    [TestMethod]
    public void TestToStringE2()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+000", str);

        bd = 12345;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+004", str);

        bd = (BigDecimal)(12345e67);
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+071", str);

        bd = (BigDecimal)(12345e-67);
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E-063", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("E2");
        Assert.AreEqual("-1.23E+071", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("E2");
        Assert.AreEqual("-1.23E-063", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E2");
        Assert.AreEqual("3.14E+000", str);
    }

    [TestMethod]
    public void TestToStringEU()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "4".ToSuperscript(), str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "71".ToSuperscript(), str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "-63".ToSuperscript(), str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "71".ToSuperscript(), str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("EU");
        Assert.AreEqual("3.1415926535897932384626433832795028841971693993751058209749445923078164"
            + "06286208998628034825342117068×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringE2U()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "0".ToSuperscript(), str);

        bd = 12345;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "4".ToSuperscript(), str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "71".ToSuperscript(), str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "-63".ToSuperscript(), str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("E2U");
        Assert.AreEqual("-1.23×10" + "71".ToSuperscript(), str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("E2U");
        Assert.AreEqual("-1.23×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E2U");
        Assert.AreEqual("3.14×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringF()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("F");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("F");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("F");
        Assert.AreEqual("1234500", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("F");
        Assert.AreEqual("12345000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)(12345e-6);
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("F");
        Assert.AreEqual("-1234500", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("F");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F");
        Assert.AreEqual(
            "3.14159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringF3()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("F3");
        Assert.AreEqual("1.234", str);

        bd = (BigDecimal)123.45;
        str = bd.ToString("F3");
        Assert.AreEqual("123.450", str);

        bd = 12345;
        str = bd.ToString("F3");
        Assert.AreEqual("12345.000", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("F3");
        Assert.AreEqual("1234500.000", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("F3");
        Assert.AreEqual("12345000.000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("F3");
        Assert.AreEqual("0.012", str);

        bd = (BigDecimal)12345e-6;
        str = bd.ToString("F3");
        Assert.AreEqual("0.012", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("F3");
        Assert.AreEqual("-1234500.000", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("F3");
        Assert.AreEqual("-0.012", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F3");
        Assert.AreEqual("3.142", str);
    }

    [TestMethod]
    public void TestToStringF0()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("F0");
        Assert.AreEqual("1", str);

        bd = (BigDecimal)12345.5;
        str = bd.ToString("F0");
        Assert.AreEqual("12346", str);

        bd = (BigDecimal)123.45;
        str = bd.ToString("F0");
        Assert.AreEqual("123", str);

        bd = 12345;
        str = bd.ToString("F0");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("F0");
        Assert.AreEqual("1234500", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("F0");
        Assert.AreEqual("12345000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = (BigDecimal)12345e-6;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("F0");
        Assert.AreEqual("-1234500", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);

        bd = BigDecimal.E;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);
    }

    [TestMethod]
    public void TestToStringG()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("G");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("G");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("G");
        Assert.AreEqual("1234500", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("G");
        Assert.AreEqual("12345000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("G");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)12345e-6;
        str = bd.ToString("G");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("G");
        Assert.AreEqual("-1234500", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("G");
        Assert.AreEqual("-0.012345", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("G");
        Assert.AreEqual("1.2345E+71", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("G");
        Assert.AreEqual("1.2345E-63", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("G");
        Assert.AreEqual("-1.2345E+71", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("G");
        Assert.AreEqual("-1.2345E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("G");
        Assert.AreEqual(
            "3.14159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringG3()
    {
        BigDecimal bd;
        string str;

        bd = 123;
        str = bd.ToString("G3");
        Assert.AreEqual("123", str);

        bd = (BigDecimal)123.45;
        str = bd.ToString("G3");
        Assert.AreEqual("123", str);

        bd = (BigDecimal)1.2345;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23", str);

        bd = 12345;
        str = bd.ToString("G3");
        Assert.AreEqual("12300", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("G3");
        Assert.AreEqual("1230000", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("G3");
        Assert.AreEqual("12300000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("G3");
        Assert.AreEqual("0.0123", str);

        bd = (BigDecimal)12345e-6;
        str = bd.ToString("G3");
        Assert.AreEqual("0.0123", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("G3");
        Assert.AreEqual("-1230000", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("G3");
        Assert.AreEqual("-0.0123", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23E+71", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23E-63", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("G3");
        Assert.AreEqual("-1.23E+71", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("G3");
        Assert.AreEqual("-1.23E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("G3");
        Assert.AreEqual("3.14", str);
    }

    [TestMethod]
    public void TestToStringGU()
    {
        BigDecimal bd;
        string str;

        bd = (BigDecimal)1.2345;
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("GU");
        Assert.AreEqual("12345", str);

        bd = (BigDecimal)12345e2;
        str = bd.ToString("GU");
        Assert.AreEqual("1234500", str);

        bd = (BigDecimal)1.2345e7;
        str = bd.ToString("GU");
        Assert.AreEqual("12345000", str);

        bd = (BigDecimal)1.2345e-2;
        str = bd.ToString("GU");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)12345e-6;
        str = bd.ToString("GU");
        Assert.AreEqual("0.012345", str);

        bd = (BigDecimal)(-12345e2);
        str = bd.ToString("GU");
        Assert.AreEqual("-1234500", str);

        bd = (BigDecimal)(-12345e-6);
        str = bd.ToString("GU");
        Assert.AreEqual("-0.012345", str);

        bd = (BigDecimal)12345e67;
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345×10⁷¹", str);

        bd = (BigDecimal)12345e-67;
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345×10⁻⁶³", str);

        bd = (BigDecimal)(-12345e67);
        str = bd.ToString("GU");
        Assert.AreEqual("-1.2345×10⁷¹", str);

        bd = (BigDecimal)(-12345e-67);
        str = bd.ToString("GU");
        Assert.AreEqual("-1.2345×10⁻⁶³", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("GU");
        Assert.AreEqual(
            "3.14159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068", str);
    }
}
