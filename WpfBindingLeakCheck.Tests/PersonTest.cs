using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace WpfBindingLeakCheck.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class PersonTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Test,STAThread]
        public void BindingAssert()
        {
            var window = new PersonWindow {DataContext = new PersonViewModel(new Person() {Name = "rimever"})};
            window.Show();
            window.Close();
            GC.Collect();
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<PersonViewModel>()).ObjectsCount));

        }
        /// <summary>
        /// 
        /// </summary>
        [Test,STAThread]
        public void CorrectBindingAssert()
        {
            ExecuteWeakPersonViewModel();
            GC.Collect();
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<PersonWindow>()).ObjectsCount));
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<WeakPersonViewModel>()).ObjectsCount));

        }

        private static void ExecuteWeakPersonViewModel()
        {
            var p = new Person() {Name = "rim"};
            foreach (var _ in Enumerable.Range(0, 2))
            {
                var window = new PersonWindow {DataContext = new WeakPersonViewModel(p)};
                window.Show();
                window.Close();
                window = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test,STAThread]
        public void BindableAssert()
        {
            foreach (var _ in Enumerable.Range(0, 2))
            {
                ExecuteBindableViewModel();
            }

            GC.Collect();
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<WeakPersonViewModel>()).ObjectsCount));
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<PersonWindow>()).ObjectsCount));
            dotMemory.Check(memory =>
                Assert.AreEqual(0, memory.GetObjects(where => where.Type.Is<BindablePerson>()).ObjectsCount));

        }

        private static void ExecuteBindableViewModel()
        {
            var window = new PersonWindow {DataContext = new BindablePerson() {Name = "rimever"}};
            window.Show();
            window.Close();
        }
    }
}
