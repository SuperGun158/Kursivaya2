namespace Zpatriot;

[TestClass]
public class UnitTest1
{
    Backend Z = new Backend();
    [TestMethod]
    public void TestSort1()
    {
        string list = "1;1;1;1;1;1;2;2;2;2;2;2;3;3;3;3;3";
        string library = "1;2;3";
        string inx1 = "0";
        string inx2 = "16";
        string boss = Z.Sort(list,library,inx1,inx2);
        Assert.IsTrue(boss == list);
    }
    [TestMethod]
    public void TestSort2()
    {
        string list = "1;1;1;1;1;1;2;2;2;2;2;2;3;3;3;3;3";
        string list2 = "3;3;3;3;3;2;2;2;2;2;2;1;1;1;1;1;1";
        string library = "3;2;1";
        string inx1 = "0";
        string inx2 = "16";
        string boss = Z.Sort(list,library,inx1,inx2);
        Assert.IsTrue(boss == list2);
    }
    [TestMethod]
    public void TestSort3()
    {
        string list = "1;1;1;1;1;1;2;2;2;2;2;2;3;3;3;3;3";
        string list2 = "2;1;3;2;1;3;3;2;2;1;1;1;2;3;2;1;3";
        string library = "1;2;3";
        string inx1 = "0";
        string inx2 = "16";
        string boss = Z.Sort(list2,library,inx1,inx2);
        Assert.IsTrue(boss == list);
    }
    [TestMethod]
    public void TestSort4()
    {
        string list = "2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string list2 = "2;1;3;2;1;3;3;2;2;1;1;1;2;3;2;1;3";
        string library = "1;2;3";
        string inx1 = "4";
        string inx2 = "8";
        string boss = Z.Sort(list2,library,inx1,inx2);
        Assert.IsTrue(boss == list);
    }
    [TestMethod]
    public void TestAdd()
    {
        string list = "2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string list2 = "1;2;3;1";
        string list3 = "2;1;3;2;1;2;3;1;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string inx = "3";
        string boss = Z.Add(list2,inx, list);
        Assert.IsTrue(boss == list3);
    }
    [TestMethod]
    public void TestAdd2()
    {
        string list = "2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string list2 = "1;2;3;1";
        string list3 = "1;2;3;1;2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string inx = "-3";
        string boss = Z.Add(list2,inx, list);
        Assert.IsTrue(boss == list3);
    }
    [TestMethod]
    public void TestAdd3()
    {
        string list = "2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3";
        string list2 = "1;2;3;1";
        string list3 = "2;1;3;2;1;2;2;3;3;1;1;1;2;3;2;1;3;1;2;3;1";
        string inx = "228";
        string boss = Z.Add(list2,inx, list);
        Assert.IsTrue(boss == list3);
    }
}