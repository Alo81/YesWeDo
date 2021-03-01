using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmashUltimateEditor;
using SmashUltimateEditor.DataTables;

namespace SmashUltimateEditorTests
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void SetValueFromName_NullNumericValues()
        {
            var dataTbl = new Battle();
            var name = "battle_time_sec";
            string val = null;

            ushort expected = 0;


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_time_sec;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetValueFromName_EmptyNumericValues()
        {
            var dataTbl = new Battle();
            var name = "battle_time_sec";
            string val = "";

            ushort expected = 0;


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_time_sec;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetValueFromName_ValidNumericValues()
        {
            var dataTbl = new Battle();
            var name = "battle_time_sec";
            string val = "5";

            ushort expected = 5;


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_time_sec;
            Assert.AreEqual(expected, actual);
        }
        /*
        [TestMethod]
        public void SetValueFromName_NegativeUnsignedNumericValues()
        {
            var dataTbl = new Battle();
            var name = "battle_time_sec";
            string val = "-1";

            Assert.ThrowsException<System.OverflowException>(() => dataTbl.SetValueFromName(name, val));

            var t = 0;
        }
        */
        [TestMethod]
        public void SetValueFromName_NegativeUnsignedNumericValues()
        {
            var dataTbl = new Battle();
            var name = "battle_time_sec";
            string val = "-5";

            ushort expected = dataTbl.battle_time_sec;


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_time_sec;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetValueFromName_NullStringValues()
        {
            var dataTbl = new Battle();
            var name = "battle_id";
            string val = null;

            string expected = "";


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_id;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetValueFromName_EmptyStringValues()
        {
            var dataTbl = new Battle();
            var name = "battle_id";
            string val = "";

            string expected = "";


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_id;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetValueFromName_ValidStringValues()
        {
            var dataTbl = new Battle();
            var name = "battle_id";
            string val = "itsa_me_mario";

            string expected = "itsa_me_mario";


            dataTbl.SetValueFromName(name, val);

            var actual = dataTbl.battle_id;
            Assert.AreEqual(expected, actual);
        }
    }
}
