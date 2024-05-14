using NUnit.Framework;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class UIInteractionTests
    {
        [Test]
        public void TestButton_Click()
        {
            Button button = new Button();
            bool buttonClicked = false;
            button.Click += (sender, e) => { buttonClicked = true; };
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            Assert.IsTrue(buttonClicked);
        }

        [Test]
        public void TestTextBox_TextInput()
        {
            TextBox textBox = new TextBox();
            string inputText = "Test input";
            textBox.Text = "";
            textBox.Focus();

            var routedEvent = TextCompositionManager.TextInputEvent;
            var args = new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                                                    new TextComposition(InputManager.Current,
                                                                        textBox, inputText));
            args.RoutedEvent = routedEvent;

            textBox.RaiseEvent(args);

            Assert.AreEqual(inputText, textBox.Text);
        }
    }
}