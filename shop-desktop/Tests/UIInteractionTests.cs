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
            // Symulacja kliknięcia przycisku
            Button button = new Button();
            bool buttonClicked = false;
            button.Click += (sender, e) => { buttonClicked = true; };
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            // Sprawdzenie, czy kliknięcie przycisku zostało obsłużone
            Assert.IsTrue(buttonClicked);
        }

        [Test]
        public void TestTextBox_TextInput()
        {
            // Symulacja wprowadzenia tekstu do pola tekstowego
            TextBox textBox = new TextBox();
            string inputText = "Test input";
            textBox.Text = "";
            textBox.Focus();

            // Tworzenie zdarzenia TextInput
            var routedEvent = TextCompositionManager.TextInputEvent;
            var args = new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                                                    new TextComposition(InputManager.Current,
                                                                        textBox, inputText));
            args.RoutedEvent = routedEvent;

            // Podnoszenie zdarzenia TextInput
            textBox.RaiseEvent(args);

            // Sprawdzenie, czy wprowadzony tekst został poprawnie zapisany w polu tekstowym
            Assert.AreEqual(inputText, textBox.Text);
        }
    }
}
